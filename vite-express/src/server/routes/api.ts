import express from 'express';
import getContractAsync from '../shared/getContractAsync';
import 'dotenv/config'

const router = express.Router();

// TODO: not secure. Only for demo purposes
router.get('/getEnv', async function(req, res, next) {
  res.send(process.env);
});

router.get('/getMetadata', async function(req, res, next) {
  const contract = await getContractAsync();
  const metadata = await contract.metadata.get();
  res.send(metadata);
});
export default router;
