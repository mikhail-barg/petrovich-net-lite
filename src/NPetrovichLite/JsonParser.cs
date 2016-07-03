using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    internal sealed class JsonParser
    {
        internal enum TokenType
        {
            EOF,
            ObjectStart,    //{
            ObjectEnd,      //}
            ArrayStart,     //[
            ArrayEnd,       //]
            PropertyName,   // "<property_name>" : 
            PropertyValueString,
            //PropertyValueNumber //no need right now
        }

        internal struct Token
        {
            internal TokenType tokenType;
            internal string stringValue;
        }

        private readonly StreamReader m_reader;
        private readonly Stack<Token> m_returnedTokens = new Stack<Token>();

        internal JsonParser(StreamReader reader)
        {
            m_reader = reader;
        }

        internal Token GetNextToken()
        {
            if (m_returnedTokens.Count > 0)
            {
                return m_returnedTokens.Pop();
            }
            return ReadTokenFromStream(false);
        }

        internal Token AssertNextTokenTypeAndConsume(TokenType type, bool pushBack = false)
        {
            Token token = GetNextToken();
            if (token.tokenType != type)
            {
                throw new ParseException(String.Format("Failed to parse. Expected {0}, got {1}", type, token.tokenType));
            }
            if (pushBack)
            {
                PushTokenBack(token);
            }
            return token;
        }

        internal string GetNextPropertyName()
        {
            return AssertNextTokenTypeAndConsume(JsonParser.TokenType.PropertyName).stringValue;
        }

        internal string GetNextPropertyValue()
        {
            return AssertNextTokenTypeAndConsume(JsonParser.TokenType.PropertyValueString).stringValue;
        }

        internal bool CheckNextTokenTypeAndConsumeIfTrue(TokenType type)
        {
            Token token = GetNextToken();
            bool result = token.tokenType == type;
            if (!result)
            {
                PushTokenBack(token);
            }
            return result;
        }

        internal void PushTokenBack(Token token)
        {
            m_returnedTokens.Push(token);
        }

        private Token ReadTokenFromStream(bool afterComma)
        {
            char startingChar;
            if (!GetNextValuableChar(out startingChar))
            {
                return new Token() { tokenType = TokenType.EOF };
            }
            switch (startingChar)
            {
            case '{':
                return new Token() { tokenType = TokenType.ObjectStart };
            case '}':
                return new Token() { tokenType = TokenType.ObjectEnd };
            case '[':
                return new Token() { tokenType = TokenType.ArrayStart };
            case ']':
                return new Token() { tokenType = TokenType.ArrayEnd };
            case '"':
                bool isPropertyName;
                string value = СontinueReadQuotedString(out isPropertyName);
                return new Token() { tokenType = isPropertyName ? TokenType.PropertyName : TokenType.PropertyValueString, stringValue = value };
            case ',':
                if (afterComma)
                {
                    throw new ParseException("Two consequent commas!");
                }
                return ReadTokenFromStream(true);
            default:
                throw new ParseException("Unexpected token starting char: '" + startingChar + "'");
            }
        }

        private readonly char[] m_buffer = new char[2048];
        private int m_bufferLength = 0;
        private int m_bufferPos = 0;

        private bool AssureNextCharExists()
        {
            if (m_bufferPos < m_bufferLength)
            {
                return true;
            }
            if (m_reader.EndOfStream)
            {
                return false;
            }
            m_bufferLength = m_reader.Read(m_buffer, 0, m_buffer.Length);
            m_bufferPos = 0;
            return true;
        }

        private void ConsumeWhitespace()
        {
            while (AssureNextCharExists())
            {
                if (!Char.IsWhiteSpace(m_buffer[m_bufferPos]))
                {
                    break;
                }
                ++m_bufferPos;
            }
        }

        private bool GetNextValuableChar(out char startingChar)
        {
            ConsumeWhitespace();
            if (!AssureNextCharExists())
            {
                startingChar = (char)0;
                return false;
            }
            startingChar = m_buffer[m_bufferPos];
            return true;
        }

        private string СontinueReadQuotedString(out bool isPropertyName)
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                if (!AssureNextCharExists())
                {
                    throw new ParseException("Not terminatred quoted string");
                }
                char nextChar = m_buffer[m_bufferPos];
                ++m_bufferPos;
                if (nextChar == '"')
                {
                    //finished reading;
                    break;
                }
                else if (nextChar == '\\')
                {
                    //escape sequence
                    nextChar = ReadEscapeSequence();
                }
                builder.Append(nextChar);
            }

            //now, look ahead, and consume a possible colon or comma
            ConsumeWhitespace();
            if (!AssureNextCharExists())
            {
                throw new ParseException("Json can't end with just a quoted string");
            }
            char followingChar = m_buffer[m_bufferPos];
            if (followingChar == ':')
            {
                isPropertyName = true;
                ++m_bufferPos;
            }
            else
            {
                isPropertyName = false;
            }

            return builder.ToString();
        }

        private char ReadEscapeSequence()
        {
            if (!AssureNextCharExists())
            {
                throw new ParseException("Unfinished escape sequence");
            }
            char followingChar = m_buffer[m_bufferPos];
            ++m_bufferPos;
            if (followingChar == 'u')
            {
                throw new ApplicationException("No support for unicode escaping yet");
            }
            return followingChar;
        }
    }
}
