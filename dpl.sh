category=services
service=history-parse
version=1.0.0

cd ./../../automation_scripts

#<project_context> <project_name> <package_version> <pack_nuget> <pack_objects> <pack_docker>
./push.sh $category $service $version no yes no