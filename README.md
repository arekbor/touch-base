## Edit frontned API address:

To update the API address in the frontend app, you need to provide the domain and port of the backend container in the `nginx.conf` file located in `Frontend/touchbase`, specifically within the proxy_pass section.

This is example:
```
location /touchbase/api {
  proxy_pass http://new_docker_domain:8080/api;
  proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
  proxy_set_header Host $http_host;
}
```

Replace new_docker_domain with the actual domain of your backend container, and adjust the port accordingly. This configuration ensures that the frontend app correctly communicates with the backend API.
