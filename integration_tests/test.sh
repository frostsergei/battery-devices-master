#!/bin/sh

client_url="http://client-app"
if curl -f --output /dev/null -s "$client_url"; then
  echo "curl $client_url test success"
else
  echo "curl $client_url failed"
  exit 1
fi

server_url="http://server-app/api/Ping"
if curl -f --output /dev/null -s "$server_url"; then
  echo "curl $server_url test success"
else
  echo "curl $server_url failed"
  exit 1
fi

request='{"content": "param:\n  type: string"}'
headers="Content-Type: application/json"
test_url="http://server-app/api/schema/parameters"
if curl -f --output /dev/null -d "$request" -H "$headers" "$test_url"; then
  echo "sending parameters test success"
else
  echo "sending parameters failed"
  exit 1
fi

if [ -f /docs/schemas/custom/parameters.yaml ]; then
  echo "parameters.yaml exists test success"
else
  echo "parameters.yaml not found"
  exit 1
fi

echo "All tests done!"

