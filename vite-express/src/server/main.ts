import express from "express";
import "express-async-errors";
import ViteExpress from "vite-express";
import apiRouter from "./routes/api";
import nftRouter from "./routes/nft";
import swaggerJsdoc from "swagger-jsdoc";
import swaggerUi from "swagger-ui-express";

const app = express();

const options = {
  definition: {
    openapi: "3.1.0",
    info: {
      title: "Express API",
      version: "0.1.0",
      description: "",
      license: {
        name: "MIT",
        url: "https://spdx.org/licenses/MIT.html",
      },
      contact: {
        name: "",
        url: "",
        email: "",
      },
    },
    servers: [
      {
        url: "http://localhost:3000",
      },
    ],
  },
  apis: ["./routes/*.ts"],
};

const specs = swaggerJsdoc(options);

app.use(
  "/api-docs",
  swaggerUi.serve,
  swaggerUi.setup(specs, { explorer: true })
);

app.use(express.json());


app.use('/api', apiRouter);
app.use('/api/nft', nftRouter);


// error handler
app.use((err: any, req: any, res: any, next: any) => {
  res.status(500).json({error: err.message})      
});


app.get("/hello", (_, res) => {
  res.send("Hello Vite + Vue + TypeScript!");
});

ViteExpress.listen(app, 3000, () =>
  console.log("Server is listening on port 3000...")
);
