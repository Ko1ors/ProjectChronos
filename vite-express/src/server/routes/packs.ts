import express from 'express';
import getContractAsync from '../shared/getContractAsync';
import getPacksContractAsync from '../shared/getPacksContractAsync';
import 'dotenv/config'
import { Request, Response, Router } from 'express';
import getPrivateSdk from '../shared/getPrivateSdk';

const router = express.Router();

// Get all Packs
router.get('/', async (req, res) => {
    const contract = await getPacksContractAsync();
    const nfts = await contract.getAll();
    res.send(nfts);
});

export default router;