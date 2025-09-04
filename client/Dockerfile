FROM node:22-slim

WORKDIR /app

COPY package*.json ./

RUN npm install --ignore-scripts

RUN npm i -g --ignore-scripts serve

COPY . .

RUN npm run build

EXPOSE 3000

CMD [ "serve", "-s", "dist" ]
