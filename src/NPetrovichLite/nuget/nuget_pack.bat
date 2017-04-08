nuget pack ..\NPetrovichLite.csproj -properties Configuration=Release -symbols

nuget push NPetrovichLite.{version}.nupkg -Source nuget.org

