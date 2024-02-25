FROM node as development

WORKDIR /app

COPY ./package.json .

RUN npm install

COPY . .

RUN npm run build

FROM nginx

COPY --from=development /app/dist  /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

CMD ["nginx", "-g", "daemon off;"]