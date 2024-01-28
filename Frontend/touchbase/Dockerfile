FROM node:18.14.1-alpine3.17 AS build
WORKDIR /usr/local/app
RUN npm cache clean --force
COPY . .
RUN npm install
RUN npm run prod

FROM nginx:latest AS ngi
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /usr/local/app/dist/touchbase /usr/share/nginx/html

WORKDIR /etc/nginx/template
COPY default.conf.template /etc/nginx/templates/default.conf.template

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]