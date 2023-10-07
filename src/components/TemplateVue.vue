<script setup lang="ts">

</script>

<template>
    <nav class="navbar navbar-expand-lg navbar-light">
        <a class="navbar-brand">Menu</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
            aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <router-link class="nav-link" to="/AuthorizationPage">Metamask authorization</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/ProfileInfo">Profile
                        <img class="profile" src="../resources/Profile.png" alt="Profile">
                    </router-link>
                </li>
                <li class="nav-item">

                    <router-link class="nav-link" to="/AboutChronos">About Chronos
                        <img class="profile" src="../resources/Profile.png" alt="Profile">
                    </router-link>
                </li>
            </ul>
        </div>
    </nav>
    <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
    <slot />
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

<style>
body {
    min-height: 100vh;
    background-color: var(--surface-ground) !important;
}
</style>

