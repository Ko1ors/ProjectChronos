<script setup lang="ts">

import { ref, onBeforeMount } from 'vue'
import type Transaction from '../models/Transaction.ts';
import TemplateVue from './TemplateVue.vue'
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type Pack, type NFT } from "@thirdweb-dev/sdk";

let packs = ref<NFT[]>([]);
let tx: Transaction;
let card = ref<NFT>({} as NFT);

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
    const contractCard = await sdk.getContract("0x72d1137eaB36EE1C9BAfB12dE74Ed683e5407508");


    packs.value = await contract.getOwned(address);

    const tokenId = 0
    // const amount = 1
    card.value = await contractCard.erc1155.get(tokenId);
})

</script>

<template>
    <TemplateVue>
        <div class="wrapper">
            <div class="card" style="width: 18rem;">
                <div class="name">{{ card.metadata.name }}</div>
                <div class="image-block">
                    <img class="card-img" :src="(card.metadata.image as string)" alt="Card image">
                    <div class="rarity-block">
                        <div class="dot"></div>
                    </div>
                    <div class="attack-block">
                        <div class="square">{{ card.metadata.power }}</div>
                    </div>
                    <div class="health-block">
                        <div class="health-dot">{{ card.metadata.health }}</div>
                    </div>
                    <div class="agility-block">
                        <div class="diamond"><span class="agility"> {{ card.metadata.agility }}</span></div>
                    </div>
                </div>
                <div>
                    <img class="class-img" src="../resources/ranged.png" alt="Card class">
                </div>
            </div>
        </div>
    </TemplateVue>
    <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
</template>

<style scoped>
.content {
    background-color: var(--surface-ground);
}

.image-block {
    position: relative;

}

.wrapper {
    padding: 20px;
}

.agility {
    transform: rotate(-45deg);
}

.dot {
    height: 35px;
    width: 35px;
    background-color: white;
    border-radius: 50%;
    display: inline-block;
}

.health-dot {
    display: flex;
    height: 35px;
    width: 35px;
    background-color: red;
    border-radius: 50%;
    align-items: center;
    justify-content: center;
    font-weight: bold;
}

.rarity-block {
    position: absolute;
    top: 17px;
    padding-left: 7px;
    padding-right: 7px;
}

.health-block {
    position: absolute;
    bottom: 7px;
    left: 45px;
    padding-left: 7px;
    padding-right: 7px;
}

.agility-block {
    position: absolute;
    bottom: 10px;
    left: 90px;
    padding-left: 7px;
    padding-right: 7px;
}

.name {
    width: 50%;
    text-align: center;
    margin: auto;
    background-color: #F2F2F2;
    border-radius: 20px;
    font-weight: bold;
}

.class-img {
    height: 40px;
    float: right;
    clear: right;
    margin: 5px;
}


.card {
    padding-top: 10px;
    overflow: hidden;
    background-color: #665FFF;
}

.card-img {
    margin-top: 10px;
    border-radius: 0%;
}

.attack-block {
    position: absolute;
    bottom: 7px;
    padding-left: 7px;
    padding-right: 7px;
}

.square {
    display: flex;
    height: 35px;
    width: 35px;
    background-color: blue;
    border-radius: 5px;
    text-align: center;
    font-weight: bold;
    align-items: center;
    justify-content: center;
}

.diamond {
    display: flex;
    height: 30px;
    width: 30px;
    background-color: greenyellow;
    border-radius: 5px;
    transform: rotate(45deg);
    font-weight: bold;
    align-items: center;
    justify-content: center;
}
</style>

