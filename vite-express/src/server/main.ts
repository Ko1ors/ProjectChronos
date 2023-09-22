import express from "express";
import ViteExpress from "vite-express";
import apiRouter from "./routes/api";

const app = express();


app.use('/api', apiRouter);

// error handler
app.use(function(err: any, req: any, res: any) {
  res.status(err.status || 500);
  // if you using view enggine
  res.render('error', {
      message: err.message,
      error: {}
  });
  // or you can use res.send();        
});

// process.on('uncaughtException', function (err) {
//   console.error(err);
// });


app.get("/hello", (_, res) => {
  res.send("Hello Vite + Vue + TypeScript!");
});

ViteExpress.listen(app, 3000, () =>
  console.log("Server is listening on port 3000...")
);
