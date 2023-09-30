import { ThirdwebSDK } from "@thirdweb-dev/sdk";

const getPrivateSdk = () => {
    return ThirdwebSDK.fromPrivateKey(process.env.PRIVATE_KEY || "", "mumbai", {
        secretKey: process.env.SECRET_KEY,
      });
}

export default getPrivateSdk;