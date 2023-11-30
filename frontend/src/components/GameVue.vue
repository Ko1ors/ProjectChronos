<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'
import { getOpponentsAsync, initiateMatchAsync } from '../api/api';
import type Opponent from '../models/Opponent';
import type MatchCard from '../models/MatchCard';
import { ref, onBeforeMount } from 'vue'
import Button from 'primevue/button'
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type NFT } from "@thirdweb-dev/sdk";
import range from "../resources/ranged.png"
import melee from "../resources/melee.png"
import Dialog from 'primevue/dialog';
import type Match from '../models/Match'

let opponents = ref<Opponent[]>([]);
let allCards = ref<NFT[]>([]);
let opponentDeckCards = ref<NFT[]>([]);
let opponentDeckContentVisible = ref(false);
let opponentNumber = ref(0)
let playersContentNone = ref(true);
let squaresContentNone = ref(false);
let buttonContentNone = ref(false);
let buttondisabled = ref(true);
let meleeName = ref("")
let rangedName = ref("")
let dialogName = ref("")
let match = ref<Match>();
let yourMelee = ref<MatchCard[]>([]);
let yourRanged = ref<MatchCard[]>([]);
let opponentsMelee = ref<MatchCard[]>([]);
let opponentsRanged = ref<MatchCard[]>([]);
let opponentsCard = ref<MatchCard>();
let yourCard = ref<MatchCard>();
let turnInfo = ref("")
let fightEffect = ref(true)
let fightEffectName = ref("")
let isEvasion = ref(false)

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
    opponentDeckCards.value = []
    opponents.value[opponentId].opponentDeck.cards.forEach((element) => {
        const activeCard = allCards.value.find((card) => {
            return parseInt(card.metadata.id) == element.cardId
        })
        opponentDeckCards.value.push(activeCard!)
    })
    opponentNumber.value = opponentId
    dialogName.value = opponents.value[opponentId].opponentDeck.name
    opponentDeckContentVisible.value = true
}

const gameView = async (opponentId: number) => {
    console.log(match);
    playersContentNone.value = false
    squaresContentNone.value = true
    opponentNumber.value = opponentId
    meleeName.value = opponents.value[opponentId].name + '\'s melee cards'
    rangedName.value = opponents.value[opponentId].name + '\'s ranged cards'
    match.value = (await initiateMatchAsync(opponentId + 1)).data!

    yourMelee.value = []
    yourRanged.value = []
    opponentsMelee.value = []
    opponentsRanged.value = []

    match.value.turns[0].cards?.forEach((element) => {
        allCards.value.forEach((card) => {
            if (element.cardId == parseInt(card.metadata.id) && card.metadata.class == "Melee") {
                yourMelee.value.push(card as MatchCard)
                yourMelee.value[yourMelee.value.length - 1].matchId = element.id
            }
        })
    })

    match.value.turns[0].cards?.forEach((element) => {
        allCards.value.forEach((card) => {
            if (element.cardId == parseInt(card.metadata.id) && card.metadata.class == "Ranged") {
                yourRanged.value.push(card as MatchCard)
                yourRanged.value[yourRanged.value.length - 1].matchId = element.id
            }
        })
    })

    match.value.turns[1].cards?.forEach((element) => {
        allCards.value.forEach((card) => {
            if (element.cardId == parseInt(card.metadata.id) && card.metadata.class == "Melee") {
                opponentsMelee.value.push(card as MatchCard)
                opponentsMelee.value[opponentsMelee.value.length - 1].matchId = element.id
            }
        })
    })

    match.value.turns[1].cards?.forEach((element) => {
        allCards.value.forEach((card) => {
            if (element.cardId == parseInt(card.metadata.id) && card.metadata.class == "Ranged") {
                opponentsRanged.value.push(card as MatchCard)
                opponentsRanged.value[opponentsRanged.value.length - 1].matchId = element.id
            }
        })
    })

    buttondisabled.value = false
}

const startTheGame = () => {
    buttonContentNone.value = true
    fightEffect.value = true

    if (yourMelee.value.find((element) => element.matchId == match.value!.turns[2].attackCardId)) {
        let index = yourMelee.value.findIndex((element) => element.matchId == match.value!.turns[2].attackCardId)
        yourCard.value = yourMelee.value.find((element) => element.matchId == match.value!.turns[2].attackCardId)
        yourMelee.value.splice(index, 1)
    }
    else {
        let index = yourRanged.value.findIndex((element) => element.matchId == match.value!.turns[2].attackCardId)
        yourCard.value = yourRanged.value.find((element) => element.matchId == match.value!.turns[2].attackCardId)
        yourRanged.value.splice(index, 1)
    }

    if (opponentsMelee.value.find((element) => element.matchId == match.value!.turns[2].targetCardId)) {
        let index = opponentsMelee.value.findIndex((element) => element.matchId == match.value!.turns[2].targetCardId)
        opponentsCard.value = opponentsMelee.value.find((element) => element.matchId == match.value!.turns[2].targetCardId)
        opponentsMelee.value.splice(index, 1)
    }
    else {
        let index = opponentsRanged.value.findIndex((element) => element.matchId == match.value!.turns[2].targetCardId)
        opponentsCard.value = opponentsRanged.value.find((element) => element.matchId == match.value!.turns[2].targetCardId)
        opponentsRanged.value.splice(index, 1)
    }

    if (match.value!.turns[2].isUserTurn) {
        turnInfo.value = "User's turn: " + yourCard.value?.metadata.name + " attaks " + opponentsCard.value?.metadata.name
    }
    else {
        turnInfo.value = "Opponent's turn: " + opponentsCard.value?.metadata.name + " attaks " + yourCard.value?.metadata.name
    }

    setTimeout(() => {
        if (match.value!.turns[2].isUserTurn) {

            if (!match.value!.turns[2].isEvaded) {
                if (match.value!.turns[2].targetHealth! > 0) {
                    opponentsCard.value!.metadata.health = match.value!.turns[2].targetHealth

                }
                else {
                    opponentsCard.value!.metadata.health = match.value!.turns[2].targetHealth = 0
                }
                isEvasion.value = false
                fightEffectName.value = "-" + match.value!.turns[2].attackDamage
                fightEffect.value = false

            }
            else {
                fightEffectName.value = "Evasion"
                isEvasion.value = true
                fightEffect.value = false

            }
        }
        else {
            if (!match.value!.turns[2].isEvaded) {
                if (match.value!.turns[2].targetHealth! > 0) {
                    yourCard.value!.metadata.health = match.value!.turns[2].targetHealth

                }
                else {
                    yourCard.value!.metadata.health = match.value!.turns[2].targetHealth = 0
                }
                isEvasion.value = false
                fightEffectName.value = "-" + match.value!.turns[2].attackDamage
                fightEffect.value = false
            }
            else {
                fightEffectName.value = "Evasion"
                isEvasion.value = true
                fightEffect.value = false
            }
        }
    }, 3000);

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

    try {
        const contractCards = await sdk.getContract(import.meta.env.VITE_CARDS_CONTRACT);
        allCards.value = await contractCards.erc1155.getAll();
    }
    catch (e) {
        location.reload()
    }

    opponents.value = (await getOpponentsAsync()).data!

})

</script>

<template>
    <TemplateVue>
        <Dialog v-model:visible="opponentDeckContentVisible" modal :header=dialogName :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <div class="card-wrapper">
                <div class="card-content">
                    <div class="cards-block">
                        <div class="row row-cols-lg-3 row-cols-md-2 row-cols-1 overflow-auto">
                            <div class="card-col" v-for="card in opponentDeckCards" :key="card.metadata.id">
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
                <br>
                <Button label="Ok" icon="pi pi-check" @click="opponentDeckContentVisible = false" autofocus />
            </template>
        </Dialog>
        <div class="wrapper">
            <div class="content">
                <Button label="Choose first opponent" class="choose-button" @click="fightEffect = !fightEffect"></Button>
                <Transition>
                    <p v-if="fightEffect" class="test-effect"> Borrow </p>
                </Transition>
                <div class="squares-block" v-bind:class="{ 'none': squaresContentNone }">
                    <div class="opponent-block">
                        <div class="opponent">
                            <div v-if="opponents[0] !== undefined" class="opponent-header">{{ opponents[0].name }}
                            </div>
                            <Button label="Choose first opponent" class="choose-button" @click="gameView(0)"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(0)"></Button>
                        </div>
                        <div class="opponent">
                            <div v-if="opponents[1] !== undefined" class="opponent-header">{{ opponents[1].name }}
                            </div>
                            <Button label="Choose second opponent" class="choose-button" @click="gameView(1)"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(1)"></Button>
                        </div>
                        <div class="opponent">
                            <div v-if="opponents[2] !== undefined" class="opponent-header">{{ opponents[2].name }}
                            </div>
                            <Button label="Choose third opponent" class="choose-button" @click="gameView(2)"></Button>
                            <Button label="Check active deck" class="choose-button" @click="opponentActiveDeck(2)"></Button>
                        </div>
                    </div>
                </div>
                <div class="player-content" v-bind:class="{ 'none': playersContentNone }">
                    <div class="player-block">
                        <div class="player-header">{{ rangedName }}</div>
                        <div class="player">
                            <div class="game-card-wrapper">
                                <div class="card-content">
                                    <div class="cards-block">
                                        <div class="row row-cols-lg-5 row-cols-md-2 row-cols-1 overflow-auto special-row">
                                            <div class="card-col" v-for="card in opponentsRanged" :key="card.metadata.id">
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
                                                            <div class="diamond"><span class="agility"> {{
                                                                card.metadata.agility
                                                            }}</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="card-element">{{
                                                            mapSymbolOfElement.get(card.metadata.element as
                                                                string)
                                                        }}</div>
                                                        <img class="class-img"
                                                            :src="getCardClass(card.metadata.class as string)"
                                                            alt="Card class">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="player-block">
                        <div class="player-header">{{ meleeName }}</div>
                        <div class="player">
                            <div class="game-card-wrapper">
                                <div class="card-content">
                                    <div class="cards-block">
                                        <div class="row row-cols-lg-5 row-cols-md-2 row-cols-1 overflow-auto special-row">
                                            <div class="card-col" v-for="card in opponentsMelee" :key="card.metadata.id">
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
                                                            <div class="diamond"><span class="agility"> {{
                                                                card.metadata.agility
                                                            }}</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="card-element">{{
                                                            mapSymbolOfElement.get(card.metadata.element as
                                                                string)
                                                        }}</div>
                                                        <img class="class-img"
                                                            :src="getCardClass(card.metadata.class as string)"
                                                            alt="Card class">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="board-content">
                        <div class="turn-info">{{ turnInfo }}</div>
                        <div class="board">
                            <Transition>
                                <p :class="{ 'evasion': isEvasion }" v-if="fightEffect" class="fight-effect"> {{
                                    fightEffectName }} </p>
                            </Transition>
                            <div class="start-button">
                                <Button label="Start the game" class="choose-button" @click="startTheGame()"
                                    :disabled="buttondisabled" v-bind:class="{ 'none': buttonContentNone }"></Button>
                            </div>
                            <div class="opponent-card">
                                <div class="card" v-if="opponentsCard !== undefined"
                                    :class="getCardRarity(opponentsCard!.metadata.rarity as string)"
                                    :style="{ backgroundColor: mapElement.get(opponentsCard!.metadata.element as string) }">
                                    <div>
                                        <div class="name">{{ opponentsCard!.metadata.name }}</div>
                                    </div>
                                    <div class="image-block">
                                        <img class="card-img" :src="(opponentsCard!.metadata.image as string)"
                                            alt="Card image">
                                        <div class="rarity-block">
                                            <div class="dot"
                                                :style="{ backgroundColor: mapRarity.get(opponentsCard!.metadata.rarity as string) }">
                                            </div>
                                        </div>
                                        <div class="attack-block">
                                            <div class="square">{{ opponentsCard!.metadata.power }}</div>
                                        </div>
                                        <div class="health-block">
                                            <div class="health-dot">{{ opponentsCard!.metadata.health }}</div>
                                        </div>
                                        <div class="agility-block">
                                            <div class="diamond"><span class="agility"> {{
                                                opponentsCard!.metadata.agility
                                            }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="card-element">{{
                                            mapSymbolOfElement.get(opponentsCard!.metadata.element as
                                                string)
                                        }}</div>
                                        <img class="class-img" :src="getCardClass(opponentsCard!.metadata.class as string)"
                                            alt="Card class">
                                    </div>
                                </div>
                            </div>
                            <div class="your-card">
                                <div class="card" v-if="yourCard !== undefined"
                                    :class="getCardRarity(yourCard!.metadata.rarity as string)"
                                    :style="{ backgroundColor: mapElement.get(yourCard!.metadata.element as string) }">
                                    <div>
                                        <div class="name">{{ yourCard!.metadata.name }}</div>
                                    </div>
                                    <div class="image-block">
                                        <img class="card-img" :src="(yourCard!.metadata.image as string)" alt="Card image">
                                        <div class="rarity-block">
                                            <div class="dot"
                                                :style="{ backgroundColor: mapRarity.get(yourCard!.metadata.rarity as string) }">
                                            </div>
                                        </div>
                                        <div class="attack-block">
                                            <div class="square">{{ yourCard!.metadata.power }}</div>
                                        </div>
                                        <div class="health-block">
                                            <div class="health-dot">{{ yourCard!.metadata.health }}</div>
                                        </div>
                                        <div class="agility-block">
                                            <div class="diamond"><span class="agility"> {{
                                                yourCard!.metadata.agility
                                            }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="card-element">{{
                                            mapSymbolOfElement.get(yourCard!.metadata.element as
                                                string)
                                        }}</div>
                                        <img class="class-img" :src="getCardClass(yourCard!.metadata.class as string)"
                                            alt="Card class">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="player-block">
                        <div class="player-header">Your melee cards</div>
                        <div class="player">
                            <div class="game-card-wrapper">
                                <div class="card-content">
                                    <div class="cards-block">
                                        <div class="row row-cols-lg-5 row-cols-md-2 row-cols-1 overflow-auto special-row">
                                            <div class="card-col" v-for="card in yourMelee" :key="card.metadata.id">
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
                                                            <div class="diamond"><span class="agility"> {{
                                                                card.metadata.agility
                                                            }}</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="card-element">{{
                                                            mapSymbolOfElement.get(card.metadata.element as
                                                                string)
                                                        }}</div>
                                                        <img class="class-img"
                                                            :src="getCardClass(card.metadata.class as string)"
                                                            alt="Card class">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="player-block">
                        <div class="player-header">Your ranged cards</div>
                        <div class="player">
                            <div class="game-card-wrapper">
                                <div class="card-content">
                                    <div class="cards-block">
                                        <div class="row row-cols-lg-5 row-cols-md-2 row-cols-1 overflow-auto special-row">
                                            <div class="card-col" v-for="card in yourRanged" :key="card.metadata.id">
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
                                                            <div class="diamond"><span class="agility"> {{
                                                                card.metadata.agility
                                                            }}</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="card-element">{{
                                                            mapSymbolOfElement.get(card.metadata.element as
                                                                string)
                                                        }}</div>
                                                        <img class="class-img"
                                                            :src="getCardClass(card.metadata.class as string)"
                                                            alt="Card class">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
.none {
    display: none;
}

.test-effect {
    color: red;
    padding: 200px 0px 0px 0px;
    font-weight: bold;
    font-size: 50px;
}

.v-leave-active {
    transition: all 2.5s ease;
}

.v-enter-from,
.v-leave-to {
    opacity: 0;
    transform: translateY(-250px);
}

.wrapper {
    padding: 20px;
}

.board-content {
    height: 600px;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    gap: 20px;
}

.your-card {
    flex: 0 0 30%;
}

.opponent-card {
    flex: 0 0 30%;
}

.board {
    height: 500px;
    width: 800px;
    background-color: green;
    border: 2px solid #000;
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 100px;
    z-index: 1;
}

.start-button {
    position: absolute;
    z-index: -1;
}

.turn-info {
    font-weight: bold;
}

.player-content {
    width: 100%
}

.player-block {
    margin: 0px 0px 30px 0px;
}

.player-block:nth-child(2) {
    margin: 0px 0px 0px 0px;
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
    height: 320px;
    width: 100%;
    overflow: auto;
}

.player-header {
    font-weight: bold;
    font-size: 20px;
    text-align: center;
}

.squares-block {
    width: 100%;
    height: 700px;
}

.name {
    text-align: center;
    font-size: 20px;
    font-weight: bold;
}

.card-wrapper {
    padding: 20px;
}

.game-card-wrapper {
    padding: 5px;
}

.card-content {
    display: flex;
    margin: 0px -10px 0px -10px;
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

.fight-effect {
    color: red;
    position: absolute;
    top: 38%;
    left: 42%;
    font-size: 50px;
    font-weight: bold;
}

.evasion {
    color: green;
}

.row {
    padding: 15px;
}

.special-row {
    width: 1100px;
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


