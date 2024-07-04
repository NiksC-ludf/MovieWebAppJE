<template>
    <div class="app-container">
        <div class="content-wrapper">
            <img alt="Vue logo" src="./assets/logo.png" class="logo">
            <router-view></router-view>
        </div>
        <RecentSearches class="recent-searches" />
    </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import RecentSearches from './components/RecentSearches.vue';

@Options({
  components: {
        RecentSearches,
  },
})
export default class App extends Vue {}
</script>

<style>
    .app-container {
        display: flex;
        justify-content: center;
        align-items: flex-start;
        height: 100vh;
        padding: 20px;
    }

    .content-wrapper {
        display: flex;
        flex-direction: column;
        align-items: center;
        flex-grow: 1;
    }

    .logo {
        margin-bottom: 20px;
    }

    .recent-searches {
        width: 250px;
        margin-left: 20px;
        margin-top: 20vh;
    }
</style>
