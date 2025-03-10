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
    const packs = await contract.getAll();
    res.send(packs);
});

// Get Owned by address Packs
router.get('/owned/:address', async (req, res) => {
    const contract = await getPacksContractAsync();
    const packs = await contract.getOwned(req.params.address);
    res.send(packs);
});

interface PackReward {
    contractAddress: string;
    tokenId: number;
    quantityPerReward: number;
    totalRewards: number;
}

const validatePackRewards = (rewards: any): rewards is PackReward[] => {
    return rewards && rewards.length > 0 && rewards.every((reward: any) => {
        return reward.contractAddress && reward.quantityPerReward && (reward.tokenId || reward.tokenId == 0) && reward.totalRewards;
    });
}

const validatePackCreation = (req: Request, res: Response) => {
    const { name, description, image, internalId, rewards, rewardsPerPack } = req.body;

    if (!name)
        return  {success: false, result: "Name is required"}
    if (!rewards)
        return {success: false, result: "Rewards is required"}
    if (!validatePackRewards(rewards))
        return {success: false, result: "Rewards is invalid"}

    // We only use ERC1155 rewards right now
    const pack = {
        // The metadata for the pack NFT itself
        packMetadata: {
            name: name,
            description: description,
            image: image,
            internalId: internalId,
        },
        // ERC20 rewards to be included in the pack
        // erc20Rewards: [
        //     {
        //         assetContract: "0x...",
        //         quantityPerReward: 5,
        //         quantity: 100,
        //         totalRewards: 20,
        //     }
        // ],
        // // ERC721 rewards to be included in the pack
        // erc721Rewards: [
        //     {
        //         assetContract: "0x...",
        //         tokenId: 0,
        //     }
        // ],
        // ERC1155 rewards to be included in the pack
        erc1155Rewards: rewards,
        openStartTime: new Date(), // the date that packs can start to be opened, defaults to now
        rewardsPerPack: rewardsPerPack ?? 1, // the number of rewards in each pack, defaults to 1
    }
    return {success: true, result: pack};
}

// Create a new Pack
router.post('/create', async (req, res) => {
    const validationResult = validatePackCreation(req, res);
    if(!validationResult.success)
        return res.status(400).send(validationResult.result);

    const sdk = getPrivateSdk();
    const contract = await getPacksContractAsync(sdk);
    const transaction = await contract.create(validationResult.result as any);
    res.send(transaction);
});

// Create a new Pack to wallet
router.post('/createToWallet', async (req, res) => {
        // Address of the wallet to create the pack to
    const { address } = req.body;
    if (!address)
        return res.status(400).send("Address is required");

    const validationResult = validatePackCreation(req, res);
    if(!validationResult.success)
        return res.status(400).send(validationResult.result);

    const sdk = getPrivateSdk();
    const contract = await getPacksContractAsync(sdk);
    const transaction = await contract.createTo(address, validationResult.result as any);
    res.send(transaction);
});

// Set approval for a pack
router.post('/setApproval', async (req, res) => {

    const sdk = getPrivateSdk();
    const edition = await  sdk.getEdition(process.env.CONTRACT_ADDRESS!)
    const result = await edition.setApprovalForAll(process.env.PACK_CONTRACT_ADDRESS!, true);
    res.send(result);
});

// Open a pack
router.post('/open', async (req, res) => {
    // The token ID of the pack you want to open
    // How many packs to open
    let { tokenId, amount } = req.body;
    if (!tokenId && tokenId !== 0)
        return res.status(400).send('Token ID is required');
    if (!amount && amount !== 0)
        amount = 1;

    const sdk = getPrivateSdk();
    const contract = await getPacksContractAsync(sdk);
    const result = await contract.open(tokenId, amount);
    res.send(result);
});

// Get pack content
router.get('/content/:tokenId', async (req, res) => {
    const contract = await getPacksContractAsync();
    const content = await contract.getPackContents(req.params.tokenId);
    res.send(content);
});

// Transfer pack
router.post('/transfer', async (req, res) => {
    // The token ID of the pack you want to send
    // The address of the wallet you want to send the pack to 
    // How many packs to transfer
    let { tokenId, address, amount } = req.body;
    if (!tokenId && tokenId !== 0)
        return res.status(400).send('Token ID is required');
    if (!address)
        return res.status(400).send('To address is required');
    if (!amount && amount !== 0)
        amount = 1;

    const sdk = getPrivateSdk();
    const contract = await getPacksContractAsync(sdk);
    const result = await contract.transfer(address, tokenId, amount);
    res.send(result);
});

export default router;