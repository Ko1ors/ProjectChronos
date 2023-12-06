<script setup lang="ts">
import Button from 'primevue/button'
import detectEthereumProvider from '@metamask/detect-provider';
import MetaMaskOnboarding from '@metamask/onboarding'
import { ref } from 'vue';
import { Buffer } from 'buffer';
import { getAuthMessagesAsync, loginAsync, logoutAsync } from '../api/api';
import type { Response } from '../api/api';
import Dialog from 'primevue/dialog';

const isMetamaskConnected = ref(false);
const connecting = ref(false);
const onboarding = new MetaMaskOnboarding();
const networkVersion = ref(0);
const accounts = ref<string[]>([]);
const sign = ref<string>("");
let messageResponse: Response<string>;
let responseInfo: string;
let logoutVisible = ref(false)

const mumbaiId = 80001;

const pageReload = () => {
  window.location.reload();
};

const logout = async () => {
  logoutVisible.value = false
  await logoutAsync()
  pageReload()
};

const connectMetamask = async () => {
  if (connecting.value) return;

  connecting.value = true;
  const provider = await detectEthereumProvider();

  if (provider) {
    // Request access to the user's MetaMask account
    await (provider as any).request({ method: 'eth_requestAccounts' });

    networkVersion.value = await (provider as any).request({ method: 'net_version' });

    // Reload the page on network change
    provider.on('chainChanged', pageReload);

    accounts.value = await (provider as any).request({ method: 'eth_requestAccounts' }).catch((err: any) => {
      if (err.code === 4001) {
        // EIP-1193 userRejectedRequest error
        // If this happens, the user rejected the connection request.
        console.error('Please connect to MetaMask.');
      } else {
        console.error(err);
      }
    });
    console.log(accounts.value);


    // Set the isMetamaskConnected flag to true
    isMetamaskConnected.value = true;
  } else {
    // If the user doesn't have MetaMask installed, display the onboarding screen
    onboarding.startOnboarding();

  }
  connecting.value = false;
  if (!(isMetamaskConnected.value && networkVersion.value != mumbaiId)) signMessageAsync();
};

const signMessageAsync = async () => {
  const provider = await detectEthereumProvider();
  // For historical reasons, you must submit the message to sign in hex-encoded UTF-8.
  // This uses a Node.js-style buffer shim in the browser.
  messageResponse = await getAuthMessagesAsync(accounts.value[0]);
  if (messageResponse.data) {
    const msg = `0x${Buffer.from(messageResponse.data, 'utf8').toString('hex')}`;
    sign.value = await (provider as any).request({
      method: 'personal_sign',
      params: [msg, accounts.value[0]],
    });
    await loginAsync(accounts.value[0], sign.value, true);
    window.open("/ProfileInfo", "_self");
  }
  else {
    responseInfo = messageResponse.message;
  }
}


</script>

<template>
  <Dialog :visible="logoutVisible" modal header="Message" :style="{ width: '50rem' }"
    :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
    <p class="m-0">
      Do you want to log out?
    </p>
    <template #footer>
      <Button label="Yes" icon="pi pi-check" @click="logout()" autofocus />
      <Button label="No" icon="pi pi-check" @click="logoutVisible = false" text />
    </template>
  </Dialog>
  <div class="meta-form">
    <h4 class="wallet-text">Connect with your wallet to access your profile</h4>
    <Button @click="connectMetamask" class="button-metamask">
      <img class="fox" src="/src/resources/MetaMask_Fox.svg.png" alt="Metamask" />
      <span>Login via metamask</span>
    </Button>
    <Button @click="logoutVisible = true" class="button-metamask" alt="Metamask">
      <span>Log out</span>
    </Button>
    <p class="error" v-if="isMetamaskConnected && networkVersion != mumbaiId">Please switch to Polygon Mumbai Testnet</p>
    <p class="error" v-if="responseInfo">Error in response: {{ responseInfo }}</p>
  </div>
</template>

<style scoped lang="scss">
.meta-form {
  border: 2px solid;
  height: 350px;
  width: 400px;
  display: flex;
  border-radius: 5px;
  background-color: var(--surface-section);
  flex-direction: column;
  padding: 20px;
  align-items: center;
  margin: auto;
  transform: translate(0, 30%);

  @include media-breakpoint-up(md) {
    width: 600px;
    height: 300px;
  }
}

.p-button {
  border-radius: 6px;
}

.error {
  margin: 20px 0px 0px 0px;
}

.button-metamask {
  width: 200px;
  height: 60px;
  margin: 30px 0px 0px 0px;

  @include media-breakpoint-up(md) {
    width: 250px;
    height: 50px;
  }
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

.content {
  background-color: var(--surface-ground);
  height: 100vh;
}

.wallet-text {
  text-align: center;
}
</style>
