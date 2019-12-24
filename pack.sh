mkdir ./out &&
	cp -R ./history-parse ./out/history-parse && cp -R ./history-parse-objects ./out/history-parse-objects &&

	rm -rf ./out/history-parse/bin/ && rm -rf ./out/history-parse/obj/ &&
	rm -rf ./out/history-parse-objects/bin/ && rm -rf ./out/history-parse-objects/obj/ &&

	cd ./out/history-parse &&

	dotnet restore -r linux-musl-x64 -v m &&
	dotnet publish -c Release -o ./../pub -r linux-musl-x64 -v m &&

	cd ./../.. &&

	docker build -t history-parse:0.1.0 -f Dockerfile . && 
	docker create --name history-parse --network cs-go-stats_prime history-parse:0.1.0 &&
	docker start history-parse

	rm -rf ./out