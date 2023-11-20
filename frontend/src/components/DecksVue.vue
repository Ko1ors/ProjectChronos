<script setup lang="ts">

import Dialog from 'primevue/dialog';
import TemplateVue from './TemplateVue.vue'
import Button from 'primevue/button'
import { ref, onBeforeMount } from 'vue'
import type Transaction from '../models/Transaction.ts';
import type Card from '../models/Card.ts';
import type DeckCard from '../models/DeckCard.ts';
import type UserDeck from '../models/UserDeck.ts';
import { MetaMaskWallet } from "@thirdweb-dev/wallets";
import { Mumbai } from "@thirdweb-dev/chains";
import { ThirdwebSDK, type NFT } from "@thirdweb-dev/sdk";
import range from "../resources/ranged.png"
import melee from "../resources/melee.png"
import { createDeckAsync, deleteDeckAsync, getActiveDeckAsync, updateDeckAsync } from '../api/api';
import { isTouchDevice } from '../shared/MobileHelper';

//let packs = ref<NFT[]>([]);
//let tx: Transaction;
let cards = ref<Card[]>([]);
let activeDeck = ref<UserDeck>();
let deleteDeckVisible = ref(false);
let updateDeckVisible = ref(false);
let isDeck = ref(true);
let buttonLable = ref("Update active deck");

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

const updateDeck = async () => {
    let deck: DeckCard[] = [];
    getList(2).forEach((card) => {
        deck.push({ cardId: parseInt(card.metadata.id), quantity: parseInt(card.quantityOwned!) })
    })
    updateDeckVisible.value = false
    if ((await getActiveDeckAsync()).data !== null) {
        activeDeck.value = (await getActiveDeckAsync()).data!;
        await updateDeckAsync(activeDeck.value.id, deck, true);
    }
    else {
        await createDeckAsync(deck, true);
        isDeck.value = true
        buttonLable.value = "Update active deck"
    }
}

const getActiveDeck = async () => {
    activeDeck.value = (await getActiveDeckAsync()).data!;
    if (activeDeck.value) {
        cards.value.forEach((element1) => {
            cards.value.forEach((element2) => {
                if (element1.metadata.id == element2.metadata.id && element1.list != element2.list) {
                    element1.quantityOwned = (parseInt(element1.quantityOwned!) + parseInt(element2.quantityOwned!)).toString()
                    cards.value.splice(cards.value.indexOf(element2), 1);
                    element1.list = 1
                }
            })
        })
        cards.value = cards.value.map((element) => { element.list = 1; return element })
        cards.value.forEach((card) => {
            activeDeck.value!.cards.forEach((deckCard) => {
                if (deckCard.cardId == parseInt(card.metadata.id) && deckCard.quantity == parseInt(card.quantityOwned!)) {
                    card.list = 2
                }
                else if (deckCard.cardId == parseInt(card.metadata.id) && deckCard.quantity != parseInt(card.quantityOwned!)) {
                    card.quantityOwned = (parseInt(card.quantityOwned!) - deckCard.quantity).toString()
                    const deckCardCopy = Object.assign({}, card)
                    deckCardCopy.quantityOwned = deckCard.quantity.toString()
                    deckCardCopy.list = 2
                    cards.value.push(deckCardCopy)
                }
            })
        })
    }
}

const deleteDeck = async () => {
    activeDeck.value = (await getActiveDeckAsync()).data!;
    if (activeDeck.value) {
        deleteDeckVisible.value = false
        await deleteDeckAsync(activeDeck.value.id);
        isDeck.value = false
        buttonLable.value = "Create a deck"
    }
    else {
        deleteDeckVisible.value = false
        isDeck.value = false
    }
}

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

const getList = (list: number) => {
    return cards.value.filter((card) => card.list == list)
}

const clearDeckList = () => {
    cards.value.forEach((element1) => {
        cards.value.forEach((element2) => {
            if (element1.metadata.id == element2.metadata.id && element1.list != element2.list) {
                element1.quantityOwned = (parseInt(element1.quantityOwned!) + parseInt(element2.quantityOwned!)).toString()
                cards.value.splice(cards.value.indexOf(element2), 1);
                element1.list = 1
            }
        })
    })
    cards.value = cards.value.map((element) => { element.list = 1; return element })
}

function getCardClass(cardClass: string) {
    if (cardClass === "Ranged") return range
    else if (cardClass === "Melee") return melee
}

const startDrag = (event: DragEvent | null, item: NFT) => {
    event!.dataTransfer!.dropEffect = "move"
    event!.dataTransfer!.effectAllowed = "move"
    event!.dataTransfer!.setData("itemID", item.metadata.id)
}

const onCardTouch = (item: Card, listId: number) => {
    if (isTouchDevice()) {
        const isInAnotherList = cards.value.find((element) => element.metadata.id == item.metadata.id && element.list == listId)
        let item2;
        if (parseInt(item!.quantityOwned!) > 1) {
            item!.quantityOwned = (parseInt(item!.quantityOwned!) - 1).toString();
            if (isInAnotherList === undefined) {
                item2 = Object.assign({}, item)
                item2.quantityOwned = "1"
                item2.list = listId
                cards.value.push(item2)
            }
            else {
                isInAnotherList!.quantityOwned = (parseInt(isInAnotherList!.quantityOwned!) + 1).toString();
            }
        }
        else {
            if (isInAnotherList === undefined) {
                item!.list = listId
            }
            else {
                isInAnotherList.quantityOwned = (parseInt(isInAnotherList!.quantityOwned!) + 1).toString();
                cards.value.splice(cards.value.indexOf(item!), 1);
            }
        }
    }
}

const onDrop = (event: DragEvent, listId: number) => {
    const itemID = event.dataTransfer?.getData("itemID")
    const item = cards.value.find((item) => item.metadata.id == itemID && item.list != listId)
    const isInAnotherList = cards.value.find((item) => item.metadata.id == itemID && item.list == listId)
    let item2;
    if (parseInt(item!.quantityOwned!) > 1) {
        item!.quantityOwned = (parseInt(item!.quantityOwned!) - 1).toString();
        if (isInAnotherList === undefined) {
            item2 = Object.assign({}, item)
            item2.quantityOwned = "1"
            item2.list = listId
            cards.value.push(item2)
        }
        else {
            isInAnotherList!.quantityOwned = (parseInt(isInAnotherList!.quantityOwned!) + 1).toString();
        }
    }
    else {
        if (isInAnotherList === undefined) {
            item!.list = listId
        }
        else {
            isInAnotherList.quantityOwned = (parseInt(isInAnotherList!.quantityOwned!) + 1).toString();
            cards.value.splice(cards.value.indexOf(item!), 1);
        }
    }
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
        cards.value = await contractCards.erc1155.getOwned(address) as Card[];
    }
    catch (e) {
        location.reload()
    }
    //packs.value = await contract.getOwned(address);
    cards.value = cards.value.map((element) => { element.list = 1; return element })

    if ((await getActiveDeckAsync()).data !== null) {
        getActiveDeck()
    }
    else {
        isDeck.value = false;
        buttonLable.value = "Create a deck"
    }
    console.log(cards);

})

</script>

<template>
    <TemplateVue>
        <Dialog :visible="deleteDeckVisible" modal header="Message" :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <p class="m-0">
                Do you want to delete the deck?
            </p>
            <template #footer>
                <Button label="Yes" icon="pi pi-check" @click="deleteDeck()" autofocus />
                <Button label="No" icon="pi pi-check" @click="deleteDeckVisible = false" text />
            </template>
        </Dialog>
        <Dialog :visible="updateDeckVisible" modal header="Message" :style="{ width: '50rem' }"
            :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
            <p class="m-0">
                Do you want to create (update) a deck?
            </p>
            <template #footer>
                <Button label="Yes" icon="pi pi-check" @click="updateDeck()" autofocus />
                <Button label="No" icon="pi pi-check" @click="updateDeckVisible = false" text />
            </template>
        </Dialog>
        <div class="wrapper">
            <div class="content">
                <div class="cards-block">
                    <div class="info-header">Your cards</div>
                    <div class="cards-content" @drop="onDrop($event, 1)" @dragenter.prevent @dragover.prevent>
                        <div class="row row-cols-lg-3 row-cols-md-2 row-cols-1 overflow-auto">
                            <div class="col" v-for="card in getList(1)" :key="card.metadata.id">
                                <div class="card" draggable="true" @dragstart="startDrag($event, card)"
                                    @click="onCardTouch(card, 2)" :class="getCardRarity(card.metadata.rarity as string)"
                                    :style="{ backgroundColor: mapElement.get(card.metadata.element as string) }">
                                    <div class="header">
                                        <div class="name">{{ card.metadata.name }}</div>
                                        <div class="count" v-if="(card.quantityOwned as unknown as number) > 1">x{{
                                            card.quantityOwned }}
                                        </div>
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
                                            <div class="diamond"><span class="agility"> {{ card.metadata.agility }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="card-element">{{ mapSymbolOfElement.get(card.metadata.element as string)
                                        }}</div>
                                        <img class="class-img" :src="getCardClass(card.metadata.class as string)"
                                            alt="Card class">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="cards-block">
                    <div class="info-header">Your decks</div>
                    <div class="cards-content" @drop="onDrop($event, 2)" @dragenter.prevent @dragover.prevent>
                        <div class="row row-cols-lg-3 row-cols-md-2 row-cols-1 overflow-auto">
                            <div class="col" v-for="card in getList(2)" :key="card.metadata.id">
                                <div class="card" draggable="true" @dragstart="startDrag($event, card)"
                                    @click="onCardTouch(card, 1)" :class="getCardRarity(card.metadata.rarity as string)"
                                    :style="{ backgroundColor: mapElement.get(card.metadata.element as string) }">
                                    <div>
                                        <div class="name">{{ card.metadata.name }}</div>
                                        <div class="count" v-if="(card.quantityOwned as unknown as number) > 1">x{{
                                            card.quantityOwned }}
                                        </div>
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
                                            <div class="diamond"><span class="agility"> {{ card.metadata.agility }}</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="card-element">{{ mapSymbolOfElement.get(card.metadata.element as string)
                                        }}</div>
                                        <img class="class-img" :src="getCardClass(card.metadata.class as string)"
                                            alt="Card class">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear-block">
                        <Button label="Clear" class="clear-button" @click="clearDeckList()"
                            :disabled="getList(2).length == 0"></Button>
                    </div>
                </div>
            </div>
            <div class="block-button">
                <Button :label="buttonLable" class="deck-button" @click="updateDeckVisible = true"
                    :disabled="getList(2).reduce((accumulator, currentValue) => accumulator + parseInt(currentValue.quantityOwned!), 0) < 5"></Button>
                <Button :disabled="!isDeck" label="Delete active deck" class="deck-button"
                    @click="deleteDeckVisible = true"></Button>
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
    margin: 0px -10px 50px -10px;
}

.cards-block {
    flex: 0 0 50%;
    padding: 0px 10px;
    z-index: 0;
}

.info-header {
    font-size: 20px;
    font-weight: bold;
    text-align: center;
}

.cards-content {
    border: 2px dashed #aaa;
    height: 650px;
    overflow: auto;
}

.clear-block {
    display: flex;
    justify-content: center;
    padding: 30px 0px 0px 0px;
}

.clear-button {
    font-size: 8px;
    height: 25px;
    width: fit-content;

    @include media-breakpoint-up(md) {
        font-size: 14px;
        height: 40px;
    }

    @include media-breakpoint-up(xl) {
        font-size: 20px;
        height: 60px;
        width: 150px;
    }
}

.block-button {
    display: flex;
    justify-content: center;
    gap: 10px;
}

.deck-button {
    font-size: 8px;
    height: 35px;
    width: fit-content;
    text-align: center;

    @include media-breakpoint-up(md) {
        font-size: 14px;
        height: 50px;
    }

    @include media-breakpoint-up(xl) {
        font-size: 20px;
        height: 70px;
        width: 250px;
    }
}

.card-element {
    display: flex;
    color: #262b32;
    height: 36px;
    float: left;
    clear: left;
    padding: 0px 0px 0px 7px;
    font-weight: bold;
    font-size: 20px;
    text-align: center;
    align-items: center;
    justify-content: center;
    font-family: 'Timmana';

    @include media-breakpoint-up(md) {
        font-size: 24px;
        padding: 1px 0px 0px 7px;
    }

    @include media-breakpoint-up(xl) {
        font-size: 40px;
        padding: 20px 0px 0px 7px;
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
    top: 10px;
    right: 4px;
    height: 14px;
    width: 14px;
    background-color: white;
    border-radius: 2px;
    text-align: center;
    align-items: center;
    justify-content: center;
    color: blue;
    font-weight: bold;
    font-size: 7px;

    @include media-breakpoint-up(md) {
        top: 11px;
        right: 4px;
        height: 16px;
        width: 16px;
        font-size: 9px;
    }

    @include media-breakpoint-up(xl) {
        top: 11px;
        right: 6px;
        height: 24px;
        width: 24px;
        font-size: 14px;
    }
}

.row {
    padding: 15px;
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
        height: 18px;
        width: 18px;
    }

    @include media-breakpoint-up(xl) {
        margin: 2px 0px 0px 2px;
        height: 30px;
        width: 30px;
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
    font-size: 9px;

    @include media-breakpoint-up(md) {
        height: 28px;
        width: 28px;
        font-size: 16px;
    }
}

.rarity-block {
    position: absolute;
    top: 10px;
    padding-left: 4px;
    padding-right: 4px;

    @include media-breakpoint-up(md) {
        top: 15px;
        padding-left: 5px;
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
        left: 35px;
    }
}

.agility-block {
    position: absolute;
    bottom: 5px;
    left: 46px;
    padding-left: 4px;
    padding-right: 4px;

    @include media-breakpoint-up(md) {
        bottom: 7px;
        padding-left: 7px;
        padding-right: 7px;
        left: 70px;
        bottom: 8px;
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
    font-size: 8px;

    @include media-breakpoint-up(md) {
        padding-left: 4px;
        padding-right: 4px;
        border: 2px solid black;
        font-size: 9px;
    }

    @include media-breakpoint-up(xl) {
        padding-left: 8px;
        padding-right: 8px;
        border: 2px solid black;
        font-size: 12px;
    }
}

.class-img {
    height: 22px;
    float: right;
    clear: right;
    margin: 7px 7px 0px 0px;

    @include media-breakpoint-up(md) {
        height: 28px;
        margin: 5px;
    }

    @include media-breakpoint-up(xl) {
        height: 45px;
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
    font-size: 9px;

    @include media-breakpoint-up(md) {
        height: 28px;
        width: 28px;
        font-size: 16px;
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
    font-size: 9px;

    @include media-breakpoint-up(md) {
        height: 26px;
        width: 26px;
        font-size: 16px;
    }
}
</style>