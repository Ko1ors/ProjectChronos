import express from "express";
import "express-async-errors";
import ViteExpress from "vite-express";
import apiRouter from "./routes/api";
import nftRouter from "./routes/nft";
import packsRouter from "./routes/packs";

const app = express();

app.use(express.json());

app.use('/api', apiRouter);
app.use('/api/nft', nftRouter);
app.use('/api/packs', packsRouter);

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
