#./pack.sh history-parse 0.x.y

service_name=$1
version=$2

if [ "$#" -ne "2" ]; then
	echo "Service name and version aren't specified." 1>&2
	exit 64
fi

rm -rf ./out &&
	mkdir ./out &&
	cp -R ./history-parse ./out/history-parse && cp -R ./history-parse-objects ./out/history-parse-objects &&

	rm -rf ./out/history-parse/bin/ && rm -rf ./out/history-parse/obj/ &&
	rm -rf ./out/history-parse-objects/bin/ && rm -rf ./out/history-parse-objects/obj/ &&

	cd ./out/history-parse &&

	dotnet restore -r linux-musl-x64 -v m &&
	dotnet publish -c Release -o ./../pub -r linux-musl-x64 -v m &&

	cd ./../..

docker stop $service_name || true
docker container rm $service_name || true
docker images -q $service_name | xargs docker image rm

docker build -t $service_name:$version -f Dockerfile . && 
	docker create --name $service_name --network cs-go-stats_prime $service_name:$version &&
	docker start $service_name

rm -rf ./out