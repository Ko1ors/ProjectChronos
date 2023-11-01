<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'

</script>

<template>
    <TemplateVue>
        <h1>Chronos</h1>
        <h3>Please select the menu section you need</h3>
    </TemplateVue>
</template>

<style scoped lang="scss">
h1 {
    text-align: center;
    font-family: 'Times New Roman', Times, serif;
    font-size: 80px;
    padding-top: 250px;

    @include media-breakpoint-up(md) {
        font-size: 140px;
    }
}

h3 {
    text-align: center;
    font-family: 'Times New Roman', Times, serif;
}
</style>

<style>
#app {
    position: relative;
}
</style>

