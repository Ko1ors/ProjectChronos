
<template>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <a class="navbar-brand">Menu</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
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
                    </router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/Decks">Decks
                    </router-link>
                </li>
            </ul>
        </div>
    </nav>


    <div class="info">
        <slot />
    </div>
    <div class="footer">&copy;<span id="year"> 2023</span><span> Chronos. All rights reserved.</span></div>
</template>

<style scoped lang="scss">
.navbar-brand {
    margin-left: 10px;
    margin-bottom: 3px;
}

.navbar {
    background-color: var(--surface-overlay);
}

.info {
    padding-bottom: 128px;
    min-height: calc(100vh - 59px);
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

.nav-item {
    margin-left: 10px;

    @include media-breakpoint-up(lg) {
        margin-left: 0px;
    }
}

.navbar-toggler {
    margin-right: 10px;
}
</style>

<style>
body {
    min-height: 100vh;
    background-color: var(--surface-ground) !important;
    position: relative;
}

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
</style>

