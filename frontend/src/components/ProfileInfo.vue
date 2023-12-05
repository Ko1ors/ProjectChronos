
<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'
import { getUserStatisticsAsync } from '../api/api';
import { ref, onBeforeMount } from 'vue'
import type UserInfo from '../models/UserInfo';

let userStatistics = ref<UserInfo>()

onBeforeMount(async () => {

    userStatistics.value = (await getUserStatisticsAsync()).data!
    console.log(userStatistics);

})

</script>

<template>
    <TemplateVue>
        <div class="wrapper">
            <div class="info">
                <p class="text">
                    User's address: {{ userStatistics?.userAddress }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    User's id: {{ userStatistics?.userId }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total user's matches: {{ userStatistics?.totalMatches }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total user's wins: {{ userStatistics?.wins }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total user's losses: {{ userStatistics?.losses }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards: {{ userStatistics?.totalOwnedCards }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of unique user's cards: {{ userStatistics?.totalUniqueOwnedCards }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Umbra" element: {{
                        userStatistics?.totalOwnedCardsByElement.items[0].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Chronos" element: {{
                        userStatistics?.totalOwnedCardsByElement.items[1].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Aether" element: {{
                        userStatistics?.totalOwnedCardsByElement.items[2].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Cryo" element: {{
                        userStatistics?.totalOwnedCardsByElement.items[3].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Common" rarity: {{
                        userStatistics?.totalOwnedCardsByRarity.items[0].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Rare" rarity: {{
                        userStatistics?.totalOwnedCardsByRarity.items[3].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Epic" rarity: {{
                        userStatistics?.totalOwnedCardsByRarity.items[1].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Legendary" rarity: {{
                        userStatistics?.totalOwnedCardsByRarity.items[2].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Melee" class: {{
                        userStatistics?.totalOwnedCardsByClass.items[1].value }}
                </p>
            </div>
            <div class="info">
                <p class="text">
                    Total count of user's cards with "Ranged" class: {{
                        userStatistics?.totalOwnedCardsByClass.items[0].value }}
                </p>
            </div>
        </div>
    </TemplateVue>
</template>

<style scoped lang="scss">
.wrapper {
    padding: 20px;
}

.info {
    margin: 0px 0px 5px 0px;
}

.text {
    font-weight: bold;
    font-size: 20px;
}
</style>

