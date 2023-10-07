<script setup lang="ts">

</script>

<template>
  <router-view></router-view>
</template>

<style scoped>
.navbar-brand {
  margin-left: 10px;
  margin-bottom: 3px;
}

.navbar {
  background-color: var(--surface-overlay);
}

.profile {
  width: 15px;
  margin-bottom: 2px;
}

.p-button {
  border-radius: 6px;
}

.section-title {
  margin-top: 50px;
  height: 150px;
}

.title {
  text-align: center;
  font-family: 'Times New Roman', Times, serif;
  font-size: 60px;
}

.content {
  background-color: var(--surface-ground);
  height: 100vh;
}

.footer {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 2rem;
  background-color: var(--surface-card);
  color: black;
  font-weight: 500;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.2rem;
}
</style>

