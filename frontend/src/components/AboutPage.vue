<!-- eslint-disable no-empty -->
<template>
    <div id="pack" style="position: absolute; top: -9999px; left: -9999px: z-index: -9999;"></div>
</template>
<script setup lang="ts">
import { onMounted } from 'vue';
let pack: any = null;
const packIds = [
    "nLfhkoB4D5k",
    "x3qR62oLEFY",
    "FKYOWRJcJ2w",
    "fcIUh4OCym8",
    "jp0p3AN9AT4",
    "OJsAOwWqXf4",
    "sZ76zPqcaAQ",
    "Wv53ATAPp70",
    "rRXORlT0xu8",
    "vMQ06GKr1kU",
    "Fmw0FDd0Krc",
    "85BVKpbDR_Y",
    "YPRnX--9kis"
]
function init() {
    try {
    pack = new (window as any).YT.Player('pack', {
        videoId: packIds[Math.floor(Math.random() * packIds.length)], 
        loop: true,
        host: 'https://www.youtube.com',
        autoplay: 1,
        events: {
            onReady: function (e: any) {
                pack.setVolume(10);
                e.target.playVideo();
            },
            onStateChange: function (e: any) {
                if (e.data === (window as any).YT.PlayerState.ENDED) {
                    pack.loadVideoById(packIds[Math.floor(Math.random() * packIds.length)]);
                }
            }
        }
    });
    } catch (e) {}
}
const startPlayer = () => {
    try {
    if(pack && pack.playVideo)
        pack.playVideo();
    } catch (e) {}
}
onMounted(() => {
    try {
    let recaptchaScript = document.createElement('script')
    recaptchaScript.setAttribute('src', 'http://www.youtube.com/player_api')
    document.head.appendChild(recaptchaScript)
    setTimeout(() => {
        init()
    }, 1000)
    setInterval(() => {
        startPlayer()
    }, 1000)
} catch (e) {}
})  
</script>