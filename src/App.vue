<script setup lang="ts">
import Button from 'primevue/button';


</script>

<template>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous">

  <div class="text-center">
    <div class="header">
      <img class="header-image" src="./resources/HeaderTitle.png" alt="Chronos">
    </div>
    <div class="row"></div>
    <div class="col-lg-4 meta-form mx-auto">
      <Button class="button-metamask">
        <img class="fox" src="./resources/MetaMask_Fox.svg.png" alt="metamask">
        <span>Login via metamask</span>
      </Button>

    </div>

  </div>
</template>

<style scoped>
.meta-form {
  border: 2px solid;
  height: 300px;
  display: flex;
  margin-top: 200px;
  border-radius: 5px;
  background-color: var(--surface-section);
}

.text-center {
  height: 100vh;
  background-color: var(--surface-ground)
}

.p-button {
  border-radius: 6px;
}

.button-metamask {
  margin: auto;
  width: 50%;
  height: 20%;
}

.button-metamask span {
  font-size: large;
  font-weight: bold;
  display: block;
  text-align: center;
  width: 100%;
}

.fox {
  width: 30px;
  height: 30px;
}

.header {
  height: 200px;
  display: flex;
}

.header-image {
  margin: auto;
}
</style>

