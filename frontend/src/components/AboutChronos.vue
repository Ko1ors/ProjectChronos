<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'

const categories = [
    {
        title: 'Features',
        features: [
            {
                title: 'Card Creation and Tokenization',
                description: 'Each card in the game is represented as a unique digital asset on the blockchain. The cards can be created and tokenized using a blockchain platform like Ethereum. Each card token will have its own unique identifier and attributes.',
            },
            {
                title: 'Smart Contracts',
                description: 'Smart contracts will be used to define the rules and behaviors of the game. They will handle functionalities like card ownership, trading, battles, and game mechanics. The smart contracts will ensure the security, transparency, and fairness of the game.',
            },
            {
                title: 'Card Distribution',
                description: 'Initially, a set of cards will be minted and distributed to players. This can be done through an initial card sale, where players purchase card packs or individual cards using cryptocurrency.',
            },
            {
                title: 'Card Trading',
                description: 'Players can trade their cards with other players through a decentralized marketplace built on the blockchain. The marketplace will utilize smart contracts to facilitate secure and transparent card transactions, allowing players to buy, sell, or exchange cards with each other.',
            },
            {
                title: 'Deck Building',
                description: 'Players will be able to create their own decks by selecting cards from their collection. The deck building process can be facilitated through a user-friendly interface that interacts with the smart contracts. Players can customize their decks based on their strategies and preferences.',
            },
            {
                title: 'Gameplay Mechanics',
                description: 'The game will implement turn-based gameplay mechanics, where players take turns using their cards to battle against opponents. The smart contracts will handle the game logic, including card effects, combat calculations, and win/loss conditions.',
            },
            {
                title: 'Game Progress and Results',
                description: 'The results of each game, including wins, losses, and rewards, will be recorded on the blockchain. This ensures that game progress and achievements are transparent, immutable, and can be verified by players.',
            },
            {
                title: 'Reward System',
                description: 'Players can earn rewards based on their performance in the game. Rewards can include in-game currency, rare cards, or other valuable items. These rewards will be recorded on the blockchain and can be traded or used to enhance the player\'s collection.',
            },
            {
                title: 'User Interface',
                description: 'A user-friendly interface will be developed to allow players to interact with the game. This interface can be a web application or a mobile app, providing players with access to their card collections, decks, battles, and the marketplace.',
            },
            {
                title: 'Community and Tournaments',
                description: 'The game can have a community aspect where players can interact, participate in tournaments, and compete for rankings. The blockchain-based nature of the game ensures fairness and transparency in tournament results.',
            },
        ],
    },
    {
        title: 'Card Elements',
        features: [
            {
                title: 'Aether',
                description: 'The element of pure energy and ethereal matter. Aether-based cards can manipulate and harness energy in various forms, such as creating powerful force fields, conjuring lightning bolts, or teleporting.',
            },
            {
                title: 'Chronos',
                description: 'The element of time manipulation. Chronos-based cards can control time, allowing players to slow down or speed up the flow of time, rewind or fast-forward events, or even glimpse into the future.',
            },
            {
                title: 'Umbra',
                description: 'The element of darkness and shadows. Umbra-based cards can manipulate shadows, blend into darkness, or unleash dark and destructive energies. They excel in stealth and subterfuge, making them masters of surprise attacks.',
            },
            {
                title: 'Cryo',
                description: 'The element of ice and frost. Cryo-based cards can freeze objects, create blizzards, or summon icy constructs. They excel in slowing down opponents and creating barriers of ice for defense.',
            },
        ],
    },
    {
        title: 'Card Classes',
        features: [
            {
                title: 'Melee',
                description: 'The Melee category represents cards that excel in close-quarters combat. These cards are skilled warriors who thrive in direct confrontations. They have strong attack values and abilities that boost their damage output or provide defensive bonuses.',
            },
            {
                title: 'Ranged',
                description: 'The Ranged category represents cards that specialize in long-range combat. These cards use bows, crossbows, or other ranged weapons to attack from a distance. They have good attack values and abilities that allow them to target specific enemy cards or provide tactical advantages.',
            },
        ],
    },
    {
        title: 'Card Attributes',
        features: [
            {
                title: 'Attack Power',
                description: 'This attribute represents the offensive strength of a card, indicating how much damage it can inflict on opposing cards or the opponent\'s health points.',
            },
            {
                title: 'Health',
                description: 'This attribute signifies the card\'s durability or resilience. It determines how much damage a card can withstand before being destroyed or removed from the game.',
            },
            {
                title: 'Speed/Agility/Evade',
                description: 'The Evade attribute represents a card\'s ability to dodge or avoid attacks from opposing cards. When a card with Evade is targeted by an opponent\'s attack, there is a chance that the attack will miss or be partially mitigated.',
            },
            {
                title: 'Rarity',
                description: 'This attribute denotes the rarity level of a card, indicating its availability and often correlating with its power level. Common, uncommon, rare, epic, and legendary are typical rarity levels used in card games.',
            },
            {
                title: 'Element',
                description: 'The Element Attribute represents the inherent elemental nature or affinity of a creature or card. It denotes the fundamental energy or essence that the creature or card possesses, which can have various effects and interactions within the game.',
            },
            {
                title: 'Class',
                description: 'A card class is a unique archetype in the vast world of card games. Each class is defined by its own aesthetics and character that describe the card and its mission in the game.',
            },
        ],
    },
    // Add more categories here (Card Classes, Card Attributes, etc.)
];


</script>

<template>
    <TemplateVue>
        <div v-for="category in categories" :key="category.title">
            <h1 class="text-center">{{ category.title }}</h1>
            <div class="row m-2 row-cols-xl-4 row-cols-lg-3 row-cols-md-2 row-cols-1">
                <div class="col" v-for="feature in category.features" :key="feature.title">
                    <div class="card mb-3">
                        <div class="card-header">{{ feature.title }}</div>
                        <div class="card-body">
                            <p class="card-text">{{ feature.description }}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </TemplateVue>
</template>

<style scoped lang="scss">
.row {
    justify-content: center;
}

.card {
    display: block;
    position: relative;
    height: 310px;
    color: var(--primary-color-text);
    background-color: var(--primary-color);
    overflow: hidden;
    z-index: 0;

    @media (min-width: 1500px) {
        height: 250px;
    }

    &:before {
        content: "";
        position: absolute;
        z-index: -1;
        top: -16px;
        right: -16px;
        background: var(--highlight-bg);
        height: 32px;
        width: 32px;
        border-radius: 32px;
        transform: scale(1);
        transform-origin: 50% 50%;
        transition: transform 0.5s ease-out;
    }

    &:hover:before {
        transform: scale(50);

    }
}

.card:hover {
    transition: all 0.3s ease-out;
    color: var(--highlight-text-color);
}
</style>

