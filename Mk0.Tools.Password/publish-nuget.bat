@ECHO OFF
DEL *.nupkg
nuget setApiKey oy2h7jlegiqdw54xzwuap56evniqy7xu3iqlasmuomjbzy
nuget pack -IncludeReferencedProjects -properties Configuration=Release
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
PAUSE