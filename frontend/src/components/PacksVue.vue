<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'
import Button from 'primevue/button'
import { ref, onBeforeMount } from 'vue'
import { ThirdwebSDK, type NFT } from "@thirdweb-dev/sdk";
import { getOwnedPacksAsync, claimPackAsync } from '../api/api';
import { PackType } from '../models/PackType'
import type Transaction from '../models/Transaction.ts';
import Dialog from 'primevue/dialog';
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import range from "../resources/ranged.png"
import melee from "../resources/melee.png"

let packs = ref<NFT[]>([]);
let claimPackVisible = ref(false)
let openPackVisible = ref(false)
let contentPackVisible = ref(false)
let choosenPack = ref<NFT>();
let allCards = ref<NFT[]>([]);
let packCards = ref<NFT[]>([]);
let transaction = ref<Transaction>();
let sdk: ThirdwebSDK

const mapElement = new Map([
    ["Aether", "#E6E6FA"],
    ["Cryo", "#428CE4"],
    ["Umbra", "#DDD"],
    ["Chronos", "#ffdc73"]
]);

const mapSymbolOfElement = new Map([
    ["Aether", "A"],
    ["Cryo", "Cr"],
    ["Umbra", "U"],
    ["Chronos", "Ch"]
]);

const mapRarity = new Map([
    ["Common", "#FFFFFF"],
    ["Rare", "#1F8FFE"],
    ["Epic", "#a335ee"],
    ["Legendary", "#FE7F27"]
]);



function getCardRarity(rarity: string) {
    switch (rarity) {
        case "Common":
            return "common-border";
        case "Rare":
            return "rare-border";
        case "Epic":
            return "epic-border";
        case "Legendary":
            return "legendary-border";
    }
}

function getCardClass(cardClass: string) {
    if (cardClass === "Ranged") return range
    else if (cardClass === "Melee") return melee
}

const claimPack = async () => {
    claimPackVisible.value = false;
    await claimPackAsync(PackType.WelcomePack)
    packs.value = (await getOwnedPacksAsync()).data!
}

const openPack = async (pack: NFT) => {
    openPackVisible.value = false

    try {
        const contractPack = await sdk.getContract(import.meta.env.VITE_PACKS_CONTRACT, "pack");
        transaction.value = await contractPack.open(pack.metadata.id, 1) as Transaction;
    }
    catch (e) {
        location.reload()
    }

    packs.value = (await getOwnedPacksAsync()).data!

    packCards.value = []
    allCards.value.forEach(element => {
        transaction.value!.erc1155Rewards!.forEach((trElement) => {
            if (trElement.tokenId == element.metadata.id) {
                packCards.value.push(element);
            }
        })
    });
    contentPackVisible.value = true
}

onBeforeMount(async () => {
    const wallet = new MetaMaskWallet({
        chains: [Mumbai],
    },);

    // connect wallet
    await wallet.connect();

    sdk = await ThirdwebSDK.fromWallet(wallet, "mumbai", {
        clientId: "b4b85c7265a5e23dae86b0085ca57083", // Use client id if using on the client side, get it from dashboard settings
    });
    const contractCards = await sdk.getContract(import.meta.env.VITE_CARDS_CONTRACT);

    packs.value = (await getOwnedPacksAsync()).data!
    allCards.value = await contractCards.erc1155.getAll();
    console.log(packs);

})

</script>

<template>
    <TemplateVue>
        <Dialog :visible="claimPackVisible" modal header="Message" :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <p class="m-0">
                Do you want to claim a pack?
            </p>
            <template #footer>
                <Button label="Yes" icon="pi pi-check" @click="claimPack()" autofocus />
                <Button label="No" icon="pi pi-check" @click="claimPackVisible = false" text />
            </template>
        </Dialog>
        <Dialog :visible="openPackVisible" modal header="Message" :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <p class="m-0">
                Do you want to open the pack?
            </p>
            <template #footer>
                <Button label="Yes" icon="pi pi-check" @click="openPack(choosenPack!)" autofocus />
                <Button label="No" icon="pi pi-check" @click="openPackVisible = false" text />
            </template>
        </Dialog>
        <Dialog v-model:visible="contentPackVisible" modal header="Pack content" :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <div class="card-wrapper">
                <div class="card-content">
                    <div class="cards-block">
                        <div class="row row-cols-lg-3 row-cols-md-2 row-cols-1 overflow-auto">
                            <div class="card-col" v-for="card in packCards" :key="card.metadata.id">
                                <div class="card" :class="getCardRarity(card.metadata.rarity as string)"
                                    :style="{ backgroundColor: mapElement.get(card.metadata.element as string) }">
                                    <div>
                                        <div class="name">{{ card.metadata.name }}</div>
                                    </div>
                                    <div class="image-block">
                                        <img class="card-img" :src="(card.metadata.image as string)" alt="Card image">
                                        <div class="rarity-block">
                                            <div class="dot"
                                                :style="{ backgroundColor: mapRarity.get(card.metadata.rarity as string) }">
                                            </div>
                                        </div>
                                        <div class="attack-block">
                                            <div class="square">{{ card.metadata.power }}</div>
                                        </div>
                                        <div class="health-block">
                                            <div class="health-dot">{{ card.metadata.health }}</div>
                                        </div>
                                        <div class="agility-block">
                                            <div class="diamond"><span class="agility"> {{ card.metadata.agility
                                            }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="card-element">{{ mapSymbolOfElement.get(card.metadata.element as
                                            string)
                                        }}</div>
                                        <img class="class-img" :src="getCardClass(card.metadata.class as string)"
                                            alt="Card class">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <template #footer>
                <Button label="Ok" icon="pi pi-check" @click="contentPackVisible = false" autofocus />
            </template>
        </Dialog>
        <div class="wrapper">
            <div class="content">
                <div class="button-block">
                    <Button label="Get Welcome pack" @click="claimPackVisible = true" class="get-button"></Button>
                </div>
                <div class="packs-block">
                    <div class="info-header">Your packs</div>
                    <div class="packs-content">
                        <div class="row row-cols-lg-6 row-cols-md-3 row-cols-1 overflow-auto">
                            <div class="col" v-for="pack in packs" :key="pack.metadata.id">
                                <div class="pack">
                                    <div class="pack-header">
                                        <div class="count" v-if="(pack.quantityOwned as unknown as number) > 1">x{{
                                            pack.quantityOwned }}
                                        </div>
                                        <div class="name-header">{{ pack.metadata.name }}</div>
                                    </div>
                                    <Button label="Open" @click="openPackVisible = true, choosenPack = pack"
                                        class="open-button"></Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </TemplateVue>
</template>

<style scoped lang="scss">
.wrapper {
    padding: 20px;
}

.content {
    display: flex;
    flex-direction: column;
}

.button-block {
    margin: 30px 0px 0px 0px;
    display: flex;
    justify-content: center;
}

.packs-block {
    margin: 30px 0px 0px 0px;
}

.info-header {
    text-align: center;
    font-size: 20px;
    font-weight: bold;
}

.packs-content {
    height: 600px;
    margin: 0px 50px 0px 50px;
    padding: 20px 20px;
    overflow: auto;
}

.get-button {
    border-radius: 10px;
}

.open-button {
    border-radius: 10px;
    margin: 70px 0px 0px 0px;
    width: fit-content;
}

.name-header {
    text-align: center;
    font-size: 22px;
    font-weight: bold;
    flex-wrap: wrap;
    padding: 40px 0px 0px 0px;
}

.pack {
    background-color: #bcd9ea;
    height: 250px;
    width: 210px;
    border-radius: 10px;
    display: flex;
    flex-direction: column;
    align-items: center;
    border: 1px solid #000;
    margin: 0px 0px 15px 0px;
}

.pack-header {
    height: 100px;
    display: flex;
    flex-direction: column;
    position: relative;
    width: 100%;
}

.col {
    display: flex;
    flex-wrap: wrap;
}

.count {
    display: flex;
    position: absolute;
    top: 10px;
    right: 6px;
    height: 25px;
    width: 25px;
    background-color: white;
    border-radius: 2px;
    text-align: center;
    justify-content: center;
    align-items: center;
    color: blue;
    font-weight: bold;
    font-size: 14px;
}




.card-wrapper {
    padding: 20px;
}

.card-content {
    display: flex;
    margin: 0px -10px 50px -10px;
}

.cards-block {
    padding: 0px 10px;
    z-index: 0;
}

.card-info-header {
    font-size: 20px;
    font-weight: bold;
    text-align: center;
}

.card-element {
    display: flex;
    color: #262b32;
    height: 36px;
    float: left;
    clear: left;
    padding: 0px 0px 0px 7px;
    font-weight: bold;
    font-size: 25px;
    text-align: center;
    align-items: center;
    justify-content: center;
    font-family: 'Timmana';
}

.card-content {
    background-color: white;
}

.common-border {
    --borderWidth: 4px;
    background: #1D1F20;
    position: relative;
    border-radius: var(--borderWidth);
}

.common-border:after {
    content: '';
    position: absolute;
    top: calc(-1 * var(--borderWidth));
    left: calc(-1 * var(--borderWidth));
    height: calc(100% + var(--borderWidth) * 2);
    width: calc(100% + var(--borderWidth) * 2);
    background: linear-gradient(60deg,
            hsl(0deg 0% 100%) 0%,
            hsl(0deg 0% 93%) 8%,
            hsl(0deg 0% 86%) 17%,
            hsl(0deg 0% 79%) 25%,
            hsl(0deg 0% 70%) 33%,
            hsl(0deg 0% 61%) 42%,
            hsl(0deg 0% 52%) 50%,
            hsl(0deg 0% 46%) 58%,
            hsl(0deg 0% 40%) 67%,
            hsl(0deg 0% 33%) 75%,
            hsl(0deg 0% 30%) 83%,
            hsl(0deg 0% 27%) 92%,
            hsl(0deg 0% 23%) 100%);
    border-radius: calc(2 * var(--borderWidth));
    z-index: -1;
    animation: animatedgradient 3s ease alternate infinite;
    background-size: 300% 300%;
}

.rare-border {
    --borderWidth: 4px;
    background: #1D1F20;
    position: relative;
    border-radius: var(--borderWidth);
}

.rare-border:after {
    content: '';
    position: absolute;
    top: calc(-1 * var(--borderWidth));
    left: calc(-1 * var(--borderWidth));
    height: calc(100% + var(--borderWidth) * 2);
    width: calc(100% + var(--borderWidth) * 2);
    background: linear-gradient(60deg,
            hsl(166deg 100% 61%) 0%,
            hsl(177deg 100% 58%) 9%,
            hsl(188deg 100% 55%) 18%,
            hsl(199deg 100% 52%) 27%,
            hsl(209deg 100% 48%) 36%,
            hsl(218deg 100% 44%) 45%,
            hsl(226deg 100% 39%) 55%,
            hsl(235deg 100% 34%) 64%,
            hsl(238deg 69% 42%) 73%,
            hsl(238deg 54% 59%) 82%,
            hsl(238deg 56% 78%) 91%,
            hsl(0deg 0% 100%) 100%);
    border-radius: calc(2 * var(--borderWidth));
    z-index: -1;
    animation: animatedgradient 3s ease alternate infinite;
    background-size: 300% 300%;
}

.epic-border {
    --borderWidth: 4px;
    background: #1D1F20;
    position: relative;
    border-radius: var(--borderWidth);
}

.epic-border:after {
    content: '';
    position: absolute;
    top: calc(-1 * var(--borderWidth));
    left: calc(-1 * var(--borderWidth));
    height: calc(100% + var(--borderWidth) * 2);
    width: calc(100% + var(--borderWidth) * 2);
    background: linear-gradient(60deg,
            hsl(183deg 100% 61%) 0%,
            hsl(209deg 100% 68%) 8%,
            hsl(232deg 100% 66%) 17%,
            hsl(243deg 100% 50%) 25%,
            hsl(264deg 100% 50%) 33%,
            hsl(274deg 100% 50%) 42%,
            hsl(282deg 100% 50%) 50%,
            hsl(295deg 100% 45%) 58%,
            hsl(309deg 100% 46%) 67%,
            hsl(319deg 100% 50%) 75%,
            hsl(330deg 100% 50%) 83%,
            hsl(341deg 100% 50%) 92%,
            hsl(354deg 100% 50%) 100%);
    border-radius: calc(2 * var(--borderWidth));
    z-index: -1;
    animation: animatedgradient 3s ease alternate infinite;
    background-size: 300% 300%;
}

.legendary-border {
    --borderWidth: 4px;
    background: #1D1F20;
    position: relative;
    border-radius: var(--borderWidth);
}

.legendary-border:after {
    content: '';
    position: absolute;
    top: calc(-1 * var(--borderWidth));
    left: calc(-1 * var(--borderWidth));
    height: calc(100% + var(--borderWidth) * 2);
    width: calc(100% + var(--borderWidth) * 2);
    background: linear-gradient(60deg,
            hsl(88deg 100% 61%) 0%,
            hsl(74deg 90% 58%) 8%,
            hsl(61deg 80% 55%) 17%,
            hsl(50deg 100% 58%) 25%,
            hsl(66deg 67% 67%) 33%,
            hsl(128deg 64% 78%) 42%,
            hsl(176deg 99% 47%) 50%,
            hsl(64deg 22% 66%) 58%,
            hsl(19deg 77% 61%) 67%,
            hsl(0deg 100% 50%) 75%,
            hsl(333deg 100% 44%) 83%,
            hsl(299deg 100% 34%) 92%,
            hsl(236deg 100% 50%) 100%);
    border-radius: calc(2 * var(--borderWidth));
    z-index: -1;
    animation: animatedgradient 3s ease alternate infinite;
    background-size: 300% 300%;
}


@keyframes animatedgradient {
    0% {
        background-position: 0% 50%;
    }

    50% {
        background-position: 100% 50%;
    }

    100% {
        background-position: 0% 50%;
    }
}

.image-block {
    position: relative;
}

.row {
    padding: 15px;
}

.card-col {
    display: flex;
    flex-wrap: wrap;
    margin: 0px 0px 20px 0px;
}

.agility {
    transform: rotate(-45deg);
}

.dot {
    height: 18px;
    width: 18px;
    border-radius: 50%;
    display: inline-block;
}

.health-dot {
    display: flex;
    height: 22px;
    width: 22px;
    background-color: #FC8080;
    border-radius: 50%;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: 12px;
}

.rarity-block {
    position: absolute;
    top: 15px;
    padding-left: 4px;
    padding-right: 4px;
}

.health-block {
    position: absolute;
    bottom: 4px;
    left: 25px;
    padding-left: 4px;
    padding-right: 4px;
}

.agility-block {
    position: absolute;
    bottom: 5px;
    left: 52px;
    padding-left: 4px;
    padding-right: 4px;
}

.name {
    text-align: center;
    background-color: white;
    border-radius: 20px;
    font-weight: bold;
    margin: auto;
    width: fit-content;
    padding-left: 4px;
    padding-right: 4px;
    border: 1px solid black;
    font-size: 12px;
}

.class-img {
    height: 25px;
    float: right;
    clear: right;
    margin: 5px 7px 0px 0px;
}


.card {
    padding-top: 10px;
}

.card-img {
    margin-top: 10px;
    border-radius: 0%;
}

.attack-block {
    position: absolute;
    bottom: 4px;
    padding-left: 4px;
    padding-right: 4px;
}

.square {
    display: flex;
    height: 22px;
    width: 22px;
    background-color: #3EBFFC;
    border-radius: 5px;
    text-align: center;
    font-weight: bold;
    align-items: center;
    justify-content: center;
    font-size: 12px;
}

.diamond {
    display: flex;
    height: 20px;
    width: 20px;
    background-color: greenyellow;
    border-radius: 5px;
    transform: rotate(45deg);
    font-weight: bold;
    align-items: center;
    justify-content: center;
    font-size: 12px;
}
</style>


