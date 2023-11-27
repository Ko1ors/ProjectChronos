<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'
import { getOpponentsAsync } from '../api/api';
import type Opponent from '../models/Opponent.ts';
import { ref, onBeforeMount } from 'vue'

let opponents = ref<Opponent[]>([]);

const getOpponents = async () => {
    opponents.value = (await getOpponentsAsync()).data!
}

onBeforeMount(async () => {
    await getOpponents()
    console.log(opponents.value[1].opponentDeck);


})

</script>

<template>
    <TemplateVue>
        <div class="wrapper">
            <div class="content">
                <div class="player">
                    <div class="name">Opponent</div>
                </div>
                <div class="board"></div>
                <div class="player">
                    <div class="name">You</div>
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
    height: 850px;
    align-items: center;
}

.player {
    flex: 0 0 25%;
    border: 1px solid #000;
    width: 1200px;

}

.board {
    flex: 0 0 50%;
    width: 1200px;
}

.name {
    text-align: center;
    font-size: 20px;
    font-weight: bold;
}
</style>


