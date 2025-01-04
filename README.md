# events

## Startup

To start the application, run this command from the root directory. Installed Docker is required.

```shell
docker compose up
```

You can access Swagger UI on http://localhost:5075/swagger.

To authenticate use predefined Admin user:

```http
POST http://localhost:5075/api/users/login HTTP/1.1
Content-Type: application/json

{
    "email": "admin@mail.com",
    "password": "admin"
}
```

You can also access:

-  `changemakerstudiosus/papercut-smtp` SMTP-Server panel on http://localhost:8080.

- `minio/minio` S3 Storage admin panel on http://localhost:9090 (username: *`admin`*, password: *`password`*)
