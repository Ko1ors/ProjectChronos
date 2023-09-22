import express from "express";
import "express-async-errors";
import ViteExpress from "vite-express";
import apiRouter from "./routes/api";

const app = express();

app.use('/api', apiRouter);

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
