import express from 'express';
import getContractAsync from '../shared/getContractAsync';
import 'dotenv/config'

const router = express.Router();

// TODO: not secure. Only for demo purposes
router.get('/', async function(req, res, next) {
    const contract = await getContractAsync();
    const nfts = await contract.erc1155.getAll();
    res.send(nfts);
});

export default router;
