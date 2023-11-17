docker-compose up -d
echo "Waiting for API to start to run EF migrations..."
pause 10
echo "Running EF migrations..."
docker container exec -it api1 bash -c /app/efbundle
echo "Done running EF migrations."