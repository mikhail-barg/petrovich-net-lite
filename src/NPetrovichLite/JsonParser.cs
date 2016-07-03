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
            return ReadTokenFromStream();
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

        private Token ReadTokenFromStream()
        {
            throw new NotImplementedException();
        }
    }
}
