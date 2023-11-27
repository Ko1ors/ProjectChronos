<script setup lang="ts">
import { onBeforeMount } from 'vue';
import { isAuthorizedAsync } from '../api/api';

const props = defineProps<{
    isAuthorized?: boolean;
}>();

onBeforeMount(async () => {
    await isAuthorizedAsync();
});
</script>