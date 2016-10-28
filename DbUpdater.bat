@echo off

pushd .\src\FlatMate.Web\

dotnet dbupdate %* --type "mysql" --host "localhost" --port "3306" --database "flatmate" --user "root" --password "admin" --scripts ".\_scripts" --backup ".\_scripts\backup"

popd