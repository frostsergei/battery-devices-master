all: generate_client

generate_client: fix_api_formatting
		nswag run clientgen.nswag

fix_api_formatting: generate_api
		sed 's/\\n[[:space:]]*"/"/g' docs/api/api.yaml > docs/api/api.g.yaml && mv docs/api/api.g.yaml docs/api/api.yaml

generate_api:
		nswag run apigen.nswag

clean:
		rm -rf client/src/build server/Build
