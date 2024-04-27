FROM alpine:3.19

RUN apk add --upgrade --no-cache \
        curl

WORKDIR tests/
COPY ["integration_tests/test.sh", "./"]
RUN chmod +x test.sh

CMD ["/tests/test.sh"]
