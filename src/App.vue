<script setup lang="ts">
import FormMetanask from './components/FormMetamask.vue'


</script>

<template>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous">

  <div class="content">
    <nav class="navbar navbar-expand-lg navbar-light">
      <a class="navbar-brand" href="#">Menu</a>
      <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
        aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav">
          <li class="nav-item active">
            <a class="nav-link" href="#">Metamask authorization</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="#">Profile
              <img class="profile" src="./resources/Profile.png" alt="Profile">
            </a>
          </li>
        </ul>
      </div>
    </nav>
    <div class="section-title">
      <h1 class="title">Welcome to chronos!</h1>
    </div>
    <FormMetanask></FormMetanask>
    <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
  </div>
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

