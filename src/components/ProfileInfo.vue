<script setup lang="ts">

import { ref, onBeforeMount } from 'vue'
import type Transaction from '../models/Transaction.ts';
import TemplateVue from './TemplateVue.vue'
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type Pack, type NFT } from "@thirdweb-dev/sdk";

let packs = ref<NFT[]>([]);
let tx: Transaction;

onBeforeMount(async () => {
    const wallet = new MetaMaskWallet({
        chains: [Mumbai],
    },);


    // connect wallet
    await wallet.connect();

    // const sdk = await ThirdwebSDK.fromWallet(wallet, "mumbai", {
    //     clientId: "b4b85c7265a5e23dae86b0085ca57083", // Use client id if using on the client side, get it from dashboard settings
    // });
    const sdk = await ThirdwebSDK.fromWallet(wallet, "mumbai", {
        clientId: "b4b85c7265a5e23dae86b0085ca57083", // Use client id if using on the client side, get it from dashboard settings
    });
    const address = "0x74a18a8512A34352211bb8FF3D3a178c62036523";

    const contract = await sdk.getContract("0x7aC679510407263603d4D17C754Fa12EEc810234", "pack");

    packs.value = await getPacks(address, contract);

    const tokenId = 0
    const amount = 1
    tx = await getTx(tokenId, amount, contract) as Transaction;
})
// Instantiate the wallet you want to use
async function getPacks(address: string, contract: Pack) {
    const packs = await contract.getOwned(address);
    return packs;
}

async function getTx(tokenId: number, amount: number, contract: Pack) {
    const tx = await contract.open(tokenId, amount);
    return tx;
}

// Instantiate the SDK using the wallet


</script>

<template>
    <TemplateVue>
        {{ packs }}
        {{ tx }}
    </TemplateVue>
    <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
</template>

<style scoped>
.content {
    background-color: var(--surface-ground);
}
</style>

