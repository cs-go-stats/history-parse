docker-compose -p cs-go-stats up --no-recreate -d

rm -rf `cat folders_to_remove | sed 's/\\r//g'`

rm -f ./../../../target/contracts/CSGOStats.Services.HistoryParse.Objects*.nupkg && 
	dotnet build -v diag -c Release --no-incremental ./history-parse.sln && 
	dotnet test -v n ./history-parse.sln && 
	dotnet pack -v m -c Release -o ./../../../target/contracts/ ./history-parse.sln && 
	dotnet nuget push ./../../../target/contracts/CSGOStats.Services.HistoryParse.Objects*.nupkg -k b8e0f6c7-0f8d-3d80-83dc-eccb59ee6083 --skip-duplicate -n true -t 30 -s http://localhost:8081/repository/nuget-default/

rm -f ./../../../target/contracts/CSGOStats.Services.HistoryParse.Objects*.nupkg

echo ''
read -p 'Run finished. Pressing any key will terminate this script.'