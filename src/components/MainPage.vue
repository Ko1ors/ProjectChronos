<script setup lang="ts">

import TemplateVue from './TemplateVue.vue'

</script>

<template>
    <TemplateVue>
        <h1>Chronos</h1>
        <h3>Please select the menu section you need</h3>
        <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
    </TemplateVue>
</template>

<style scoped>
h1 {
    text-align: center;
    font-family: 'Times New Roman', Times, serif;
    font-size: 140px;
    padding-top: 250px;
}

h3 {
    text-align: center;
    font-family: 'Times New Roman', Times, serif;
}
</style>

<style>
.footer {
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    padding: 2rem;
    background-color: var(--surface-card);
    color: black;
    font-weight: 500;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 80px;
}

#app {
    position: relative;
}
</style>

