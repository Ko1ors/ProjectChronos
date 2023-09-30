import express from 'express';
import getContractAsync from '../shared/getContractAsync';
import 'dotenv/config'
import { Request, Response, Router } from 'express';
import getPrivateSdk from '../shared/getPrivateSdk';

const elements = ["Chronos", "Gaia", "Aether"] as const;
export type Element = typeof elements[number];

const classes = ["Melee", "Ranged"] as const;
export type CardClass = typeof classes[number];

const rarity = ["Common", "Rare", "Epic", "Legendary"] as const;
export type Rarity = typeof rarity[number];

const validateElement = (element: any): element is Element => {
    return elements.includes(element);
}

const validateClass = (classType: any): classType is CardClass => {
    return classes.includes(classType);
}

const validateRarity = (rarityType: any): rarityType is Rarity => {
    return rarity.includes(rarityType);
}

interface NFTMetadata {
  name: string;
  description: string;
  image: string;
  element: Element;
  class: CardClass;
  power: number;
  health: number;
  agility: number;
  rarity: Rarity;
}

const router = express.Router();

// Get all NFTs
router.get('/', async (req, res) => {
    const contract = await getContractAsync();
    const nfts = await contract.erc1155.getAll();
    res.send(nfts);
});

// Get NFTs count
router.get('/count', async (req, res) => {
    const contract = await getContractAsync();
    const count = await contract.erc1155.totalCount();
    res.send(count);
});

// Mint a new NFT
router.post('/mint', async (req: Request<NFTMetadata>, res) => {
    const metadata = req.body as NFTMetadata;

    if(!metadata) 
        return res.status(400).send('Metadata is required');
    if(!metadata.name)
        return res.status(400).send('Name is required');
    if(!metadata.description)
        return res.status(400).send('Description is required');
    if(!metadata.image)
        return res.status(400).send('Image is required');
    if(!validateElement(metadata.element))
        return res.status(400).send('Element is required');
    if(!validateClass(metadata.class))
        return res.status(400).send('Class is required');
    if(!metadata.power)
        return res.status(400).send('Power is required');
    if(!metadata.health)
        return res.status(400).send('Health is required');
    if(!metadata.agility)
        return res.status(400).send('Agility is required');
    if(!validateRarity(metadata.rarity))
        return res.status(400).send('Rarity is required');

    // Custom metadata of the NFTs to create
    const metadatas = [metadata];
    // Example:
    // const metadatas = [{
    //     name: "Cool NFT",
    //     description: "This is a cool NFT",
    //     image: fs.readFileSync("path/to/image.png"), // This can be an image url or file
    // }, {
    //     name: "Cool NFT",
    //     description: "This is a cool NFT",
    //     image: fs.readFileSync("path/to/image.png"),
    // }];

    // Use private sdk to sign the transaction
    const sdk = getPrivateSdk();
    const contract = await getContractAsync(sdk);
    const results = await contract.erc1155.lazyMint(metadatas); // uploads and creates the NFTs on chain
    res.send(results);
});

// Transfer an NFT
router.post('/transfer', async (req, res) => {
    // The token ID of the NFT you want to send
    // The address of the wallet you want to send the NFT to 
    // How many copies of the NFTs to transfer
    let { tokenId, address, amount } = req.body;
    if(!tokenId && tokenId !== 0)
        return res.status(400).send('Token ID is required');
    if(!address)
        return res.status(400).send('To address is required');
    if(!amount && amount !== 0)
        amount = 1;

        
    // Use private sdk to sign the transaction
    const sdk = getPrivateSdk();
    const contract = await getContractAsync(sdk);
    const result = await contract.erc1155.transfer(address, tokenId, amount);
    res.send(result);
});



export default router;
