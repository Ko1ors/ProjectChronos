<script setup lang="ts">
import Button from 'primevue/button'
import detectEthereumProvider from '@metamask/detect-provider';
import MetaMaskOnboarding from '@metamask/onboarding'
//import { MetaMaskSDK } from '@metamask/sdk';
import { ref } from 'vue';
import { Buffer } from 'buffer';
import { getAuthMessagesAsync, loginAsync } from '../api/api';
import type { Response } from '../api/api';

// const options = {
//     injectProvider: false,
//     communicationLayerPreference: 'webrtc',
// };
// const MMSDK = new MetaMaskSDK(options);

// const ethereum = MMSDK.getProvider();
const isMetamaskConnected = ref(false);
const connecting = ref(false);
const onboarding = new MetaMaskOnboarding();
const networkVersion = ref(0);
const accounts = ref<string[]>([]);
const sign = ref<string>("");
const message = ref<string>("test message");
let messageResponse: Response<string>;
let responseInfo: string;

const mumbaiId = 80001;

const handleChainChanged = () => {
  window.location.reload();
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
    provider.on('chainChanged', handleChainChanged);

    accounts.value = await (provider as any).request({ method: 'eth_requestAccounts' }).catch((err: any) => {
      if (err.code === 4001) {
        // EIP-1193 userRejectedRequest error
        // If this happens, the user rejected the connection request.
        console.error('Please connect to MetaMask.');
      } else {
        console.error(err);
      }
    });

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
    message.value = messageResponse.data;
    const msg = `0x${Buffer.from(message.value, 'utf8').toString('hex')}`;
    sign.value = await (provider as any).request({
      method: 'personal_sign',
      params: [msg, accounts.value[0]],
    });
    await loginAsync(accounts.value[0], sign.value, true);
  }
  else {
    responseInfo = messageResponse.message;
  }
}


</script>

<template>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />

  <div class="col-lg-4 meta-form">
    <h4 class="wallet-text">Connect with your wallet to access your profile</h4>
    <Button @click="connectMetamask" class="button-metamask">
      <img class="fox" src="/src/resources/MetaMask_Fox.svg.png" alt="Metamask" />
      <span>Login via metamask</span>
    </Button>
    <p v-if="isMetamaskConnected && networkVersion != mumbaiId">Please switch to Polygon Mumbai Testnet</p>
    <p v-if="responseInfo">Error in response: {{ responseInfo }}</p>
  </div>
</template>

<style scoped lang="scss">
.meta-form {
  border: 2px solid;
  height: 300px;
  display: flex;
  margin-top: 50px;
  border-radius: 5px;
  background-color: var(--surface-section);
  flex-direction: column;
  padding: 20px;
  align-items: center;
  margin-left: 20px;
  margin-right: 20px;

  @include media-breakpoint-up(md) {
    margin-left: 50px;
    margin-right: 50px;
  }

  @include media-breakpoint-up(lg) {
    margin-left: auto;
    margin-right: auto;
  }
}

.p-button {
  border-radius: 6px;
}

.button-metamask {
  width: 50%;
  height: 20%;
  margin-top: 80px;
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
