<script setup lang="ts">

import FormMetamask from './FormMetamask.vue'
import TemplateVue from './TemplateVue.vue'

</script>

<template>
    <TemplateVue>
        <div class="content">
            <div class="section-title">
                <h1 class="title">Welcome to chronos!</h1>
            </div>
            <FormMetamask></FormMetamask>
        </div>
    </TemplateVue>
</template>

<style scoped>
.p-button {
    border-radius: 6px;
}

.section-title {
    height: 150px;
}

.title {
    text-align: center;
    font-family: 'Times New Roman', Times, serif;
    font-size: 60px;
}

.content {
    background-color: var(--surface-ground);
}
</style>

