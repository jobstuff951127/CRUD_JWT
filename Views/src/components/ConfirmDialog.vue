<template>
	<v-dialog v-model="showConfirmDialog" max-width="280">
		<v-card>
			<v-card-title class="headline" v-if="!confirmDialogCondition">Activate item?</v-card-title>
			<v-card-title class="headline" v-else>Deactivate Item?</v-card-title>

			<v-card-text>
				You're about to
				<span class="font-weight-bold" v-if="!confirmDialogCondition">activate</span>
				<span class="font-weight-bold" v-else>deactivate</span>
				this item:
				<span class="font-weight-bold">{{ confirmDialogName }}</span>
			</v-card-text>
			<v-card-actions>
				<v-spacer></v-spacer>
				<v-btn color="red" @click="showConfirmDialog = !showConfirmDialog" text>Cancel</v-btn>
				<v-btn
					color="primary"
					@click="updateConfirmDialog"
					:loading="loaderConditionSave"
					text
				>Accept</v-btn>
			</v-card-actions>
		</v-card>
	</v-dialog>
</template>

<script>
	export default {
		name: "ConfirmDialog",
		props: [
			"showConfirmDialog",
			"confirmDialogCondition",
			"confirmDialogName",
			"updateConfirmDialog",
			"loaderConditionSave",
		],

		data: () => ({}),

		computed: {},

		watch: {},

		created() {},

		methods: {},
	};
</script>