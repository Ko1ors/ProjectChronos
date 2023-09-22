import express from 'express';
import getContractAsync from '../shared/getContractAsync';

const router = express.Router();

router.get('/getMetadata', async function(req, res, next) {
  const contract = await getContractAsync();
  const metadata = await contract.metadata.get();
  res.send(metadata);
});
export default router;
