<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'
import { getOpponentsAsync } from '../api/api';
import type Opponent from '../models/Opponent.ts';
import { ref, onBeforeMount } from 'vue'
import Button from 'primevue/button'
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type NFT } from "@thirdweb-dev/sdk";
import type UserDeck from '../models/UserDeck.ts';
import { getActiveDeckAsync } from '../api/api';
import range from "../resources/ranged.png"
import melee from "../resources/melee.png"

let opponents = ref<Opponent[]>([]);
let opponentName = ref("Opponent");
let userCards = ref<NFT[]>([]);
let deckCards = ref<NFT[]>([]);
let activeDeck = ref<UserDeck>();

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

const opponentActiveDeck = (opponentId: number) => {

}

onBeforeMount(async () => {
    const wallet = new MetaMaskWallet({
        chains: [Mumbai],
    },);


    // connect wallet
    await wallet.connect();

    const sdk = await ThirdwebSDK.fromWallet(wallet, "mumbai", {
        clientId: "b4b85c7265a5e23dae86b0085ca57083", // Use client id if using on the client side, get it from dashboard settings
    });
    const address = await wallet.getAddress();

    //const contract = await sdk.getContract(import.meta.env.VITE_PACKS_CONTRACT, "pack");
    try {
        const contractCards = await sdk.getContract(import.meta.env.VITE_CARDS_CONTRACT);
        userCards.value = await contractCards.erc1155.getAll();
    }
    catch (e) {
        location.reload()
    }

    opponents.value = (await getOpponentsAsync()).data!
    activeDeck.value = (await getActiveDeckAsync()).data!;
    // activeDeck.value.cards.forEach((element) => {
    //     for (let index = 0; index < element.quantity; index++) {
    //         const activeCard = userCards.value.find((card) => {
    //             return parseInt(card.metadata.id) == element.cardId
    //         })
    //         activeCard!.quantityOwned = "1"
    //         deckCards.value.push(activeCard!)
    //     }
    // })

    console.log(userCards);



})

</script>

<template>
    <TemplateVue>
        <div class="wrapper">
            <div class="content">
                <div class="player-header">{{ opponentName }}</div>
                <div class="player">
                </div>
                <div class="game-block">
                    <div class="opponent-block">
                        <div class="opponent">
                            <div class="opponent-header">Opponent 1</div>
                            <Button label="Choose first opponent" class="choose-button"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(0)"></Button>
                        </div>
                        <div class="opponent">
                            <div class="opponent-header">Opponent 2</div>
                            <Button label="Choose second opponent" class="choose-button"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(1)"></Button>
                        </div>
                        <div class="opponent">
                            <div class="opponent-header">Opponent 3</div>
                            <Button label="Choose third opponent" class="choose-button"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(2)"></Button>
                        </div>
                    </div>
                    <div class="board"></div>
                </div>
                <div class="player-header">You</div>
                <div class="player">
                    <!-- <div class="card-wrapper">
                        <div class="card-content">
                            <div class="cards-block">
                                <div class="row row-cols-lg-5 row-cols-md-3 row-cols-1 overflow-auto">
                                    <div class="card-col" v-for="card in deckCards" :key="card.metadata.id">
                                        <div class="card" :class="getCardRarity(card.metadata.rarity as string)"
                                            :style="{ backgroundColor: mapElement.get(card.metadata.element as string) }">
                                            <div>
                                                <div class="name">{{ card.metadata.name }}</div>
                                            </div>
                                            <div class="image-block">
                                                <img class="card-img" :src="(card.metadata.image as string)"
                                                    alt="Card image">
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
                    </div> -->
                </div>
            </div>
        </div>
    </TemplateVue>
</template>

<style scoped lang="scss">
.wrapper {
    padding: 20px;
}

.choose-button {
    width: 240px;
    border-radius: 20px;
}

div.opponent-header {
    font-weight: bold;
    font-size: 20px;
}

div.opponent-block {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 50px;
    height: 100%;
}

div.opponent {
    width: 300px;
    height: 300px;
    background-color: yellow;
    display: flex;
    flex-direction: column;
    gap: 20px;
    align-items: center;
    justify-content: center;
}

.content {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.player {
    border: 1px solid #000;
    height: 300px;
    width: 100%;
    overflow: auto;
}

.player-header {
    font-weight: bold;
    font-size: 20px;
}

.game-block {
    height: 500px;
    width: 100%;
}

.name {
    text-align: center;
    font-size: 20px;
    font-weight: bold;
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


