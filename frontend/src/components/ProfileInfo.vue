<script setup lang="ts">

import { ref, onBeforeMount } from 'vue'
import type Transaction from '../models/Transaction.ts';
import TemplateVue from './TemplateVue.vue'
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type Pack, type NFT } from "@thirdweb-dev/sdk";

let packs = ref<NFT[]>([]);
let tx: Transaction;
let cards = ref<NFT[]>([]);
let mapElement = new Map([
    ["Aether", "#E6E6FA"],
    ["Cryo", "#428CE4"],
    ["Umbra", "#DDD"],
    ["Chronos", "#ffdc73"]
]);

let mapSymbolOfElement = new Map([
    ["Aether", "A"],
    ["Cryo", "Cr"],
    ["Umbra", "U"],
    ["Chronos", "Ch"]
]);

let mapRarity = new Map([
    ["Common", "white"],
    ["Rare", "blue"],
    ["Epic", "purple"],
    ["Legendary", "orange"]
]);


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
    const contractCards = await sdk.getContract("0x72d1137eaB36EE1C9BAfB12dE74Ed683e5407508");


    packs.value = await contract.getOwned(address);

    const tokenId = 0
    // const amount = 1
    cards.value = await contractCards.erc1155.getOwned(address);
})

</script>

<template>
    <TemplateVue>
        <div class="row row-cols-lg-6 row-cols-md-3 row-cols-2 overflow-auto">
            <div class="col" v-for="card in cards" :key="card.metadata.id">
                <div class="card" :class="{
                    'epic-border': card.metadata.rarity === 'Epic',
                    'common-border': card.metadata.rarity === 'Common',
                    'rare-border': card.metadata.rarity === 'Rare',
                    'legendary-border': card.metadata.rarity === 'Legendary'
                }" :style="{ backgroundColor: mapElement.get(card.metadata.element as string) }">
                    <div class="header">
                        <div class="name">{{ card.metadata.name }}</div>
                        <div class="count" v-if="(card.quantityOwned as unknown as number) > 1">x{{ card.quantityOwned }}
                        </div>
                    </div>
                    <div class="image-block">
                        <img class="card-img" :src="(card.metadata.image as string)" alt="Card image">
                        <div class="rarity-block">
                            <div class="dot" :style="{ backgroundColor: mapRarity.get(card.metadata.rarity as string) }">
                            </div>
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
                        <div class="card-element">{{ mapSymbolOfElement.get(card.metadata.element as string) }}</div>
                        <img class="class-img" v-if="card.metadata.class === 'Ranged'" src="../resources/ranged.png"
                            alt="Card class">
                        <img class="class-img" v-if="card.metadata.class === 'Melee'" src="../resources/melee.png"
                            alt="Card class">
                    </div>
                </div>
            </div>
        </div>
    </TemplateVue>
</template>

<style scoped lang="scss">
.card-element {
    display: flex;
    color: #262b32;
    height: 36px;
    float: left;
    clear: left;
    margin-left: 8px;
    margin-top: 5px;
    font-weight: bold;
    font-size: 25px;
    text-align: center;
    align-items: center;
    justify-content: center;
    font-family: 'Timmana';

    @include media-breakpoint-up(md) {
        font-size: 36px;
        margin-left: 10px;
        margin-top: 8px;
    }
}

.content {
    background-color: var(--surface-ground);
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

.count {
    display: flex;
    position: absolute;
    top: 13px;
    right: 4px;
    height: 12px;
    width: 12px;
    background-color: white;
    border-radius: 2px;
    text-align: center;
    align-items: center;
    justify-content: center;
    color: blue;
    font-weight: bold;
    font-size: 10px;

    @include media-breakpoint-up(md) {
        top: 8px;
        right: 5px;
        height: 28px;
        width: 28px;
        font-size: 1rem;
    }
}

.row {
    padding: 30px;
}

.col {
    display: flex;
    flex-wrap: wrap;
    margin-bottom: 20px;
}

.agility {
    transform: rotate(-45deg);
}

.dot {
    height: 13px;
    width: 13px;
    border-radius: 50%;
    display: inline-block;

    @include media-breakpoint-up(md) {
        height: 35px;
        width: 35px;
    }
}

.health-dot {
    display: flex;
    height: 18px;
    width: 18px;
    background-color: #FC8080;
    border-radius: 50%;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: 12px;

    @include media-breakpoint-up(md) {
        height: 35px;
        width: 35px;
        font-size: 1rem;
    }
}

.rarity-block {
    position: absolute;
    top: 10px;
    padding-left: 4px;
    padding-right: 4px;

    @include media-breakpoint-up(md) {
        top: 17px;
        padding-left: 7px;
        padding-right: 7px;
    }
}

.health-block {
    position: absolute;
    bottom: 4px;
    left: 23px;
    padding-left: 4px;
    padding-right: 4px;

    @include media-breakpoint-up(md) {
        bottom: 7px;
        padding-left: 7px;
        padding-right: 7px;
        left: 45px;
    }
}

.agility-block {
    position: absolute;
    bottom: 4px;
    left: 46px;
    padding-left: 4px;
    padding-right: 4px;

    @include media-breakpoint-up(md) {
        bottom: 7px;
        padding-left: 7px;
        padding-right: 7px;
        left: 90px;
    }
}

.name {
    text-align: center;
    background-color: white;
    border-radius: 20px;
    font-weight: bold;
    margin: auto;
    width: fit-content;
    padding-left: 2px;
    padding-right: 2px;
    border: 1px solid black;
    font-size: 10px;

    @include media-breakpoint-up(md) {
        padding-left: 12px;
        padding-right: 12px;
        border: 2px solid black;
        font-size: 1rem;
    }
}

.class-img {
    height: 28px;
    float: right;
    clear: right;
    margin-right: 7px;
    margin-top: 8px;

    @include media-breakpoint-up(md) {
        height: 40px;
        margin: 5px;
    }
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

    @include media-breakpoint-up(md) {
        bottom: 7px;
        padding-left: 7px;
        padding-right: 7px;
    }
}

.square {
    display: flex;
    height: 18px;
    width: 18px;
    background-color: #3EBFFC;
    border-radius: 5px;
    text-align: center;
    font-weight: bold;
    align-items: center;
    justify-content: center;
    font-size: 12px;

    @include media-breakpoint-up(md) {
        height: 35px;
        width: 35px;
        font-size: 1rem;
    }
}

.diamond {
    display: flex;
    height: 16px;
    width: 16px;
    background-color: greenyellow;
    border-radius: 5px;
    transform: rotate(45deg);
    font-weight: bold;
    align-items: center;
    justify-content: center;
    font-size: 12px;

    @include media-breakpoint-up(md) {
        height: 30px;
        width: 30px;
        font-size: 1rem;
    }
}
</style>

