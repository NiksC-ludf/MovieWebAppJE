<template>
    <div class="recent-searches">
        <h2>Recent Searches</h2>
        <ul>
            <li v-for="query in recentSearches" :key="query">
                <p>{{ query }}</p>
            </li>
        </ul>
    </div>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue';
import { useStore } from 'vuex';

export default defineComponent({
  name: 'RecentSearches',
  setup() {
    const store = useStore();
    const recentSearches = computed(() => store.state.recentSearches);

    return {
      recentSearches,
    };
  },
});
</script>

<style scoped>
    .recent-searches {
        position: sticky;
        top: 20px;
        width: 250px; /* Find a width that balances well with your main content */
        height: auto;
        margin-left: 20px; /* Add some space between the main content and the sidebar */
        padding: 10px;
        overflow-y: auto;
        z-index: 1000;
        background-color: #f9f9f9;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1); 
    }


    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        margin-bottom: 5px;
    }

    router-link {
        color: #007bff;
        text-decoration: none;
        cursor: pointer;
    }

        router-link:hover {
            text-decoration: underline;
        }
</style>
