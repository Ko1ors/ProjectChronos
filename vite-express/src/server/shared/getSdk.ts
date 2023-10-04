import { ThirdwebSDK } from "@thirdweb-dev/sdk";

const getSdk = () => {
    return new ThirdwebSDK("mumbai", {
        secretKey: process.env.SECRET_KEY,
      });
}

export default getSdk;