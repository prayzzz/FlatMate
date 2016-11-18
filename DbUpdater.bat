@echo off

pushd .\src\FlatMate.Web\

dotnet dbupdate %* --type "mssql" --host "(localdb)\MSSQLLocalDB" --isecurity "true" --database "flatmate" --scripts ".\_scripts" --backup ".\_scripts\backup"

popd